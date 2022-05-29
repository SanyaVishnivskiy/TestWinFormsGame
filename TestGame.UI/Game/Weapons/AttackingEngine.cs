namespace TestGame.UI.Game.Weapons;

public class AttackingEngine : IDisposable
{
    private readonly Dictionary<Guid, HashSet<Guid>> _attackedEntitiesByWeapon = new();
    private readonly GameState _state;

    private IReadOnlyList<Entity> _attackingEntities;
    private CollisionDetector _collisionDetector;

    public AttackingEngine(GameState state)
    {
        _state = state;
        _state.OnMarkingAttack += OnWeaponActivation;
        OnWeaponActivation(this, new WeaponActivationEventArgs(_state.AttackingEntities));

        _collisionDetector = new CollisionDetector(_state.Map);
    }

    private void OnWeaponActivation(object? sender, WeaponActivationEventArgs e)
    {
        _attackingEntities = e.Weapons;
    }

    public void ProcessAttacks()
    {
        var attacks = UpdateAttacksTick();
        ApplyDamage(attacks);
    }

    private List<AttackInfo> UpdateAttacksTick()
    {
        var attacks = new List<AttackInfo>();
        foreach (var entity in _attackingEntities.ToList())
        {
            var previousPosition = entity.CurrentPosition.Clone();

            var details = entity.AttackTick();

            if (details is null)
            {
                continue;
            }

            if (details.AttackFinished)
            {
                _attackedEntitiesByWeapon.Remove(details.AttackId);
                continue;
            }

            if (entity.Weapon.Attacking)
            {
                var currentPosition = entity.CurrentPosition.Clone();
                attacks.Add(new AttackInfo(details.AttackId, entity, previousPosition, currentPosition));
            }
        }

        return attacks;
    }

    private void ApplyDamage(List<AttackInfo> attacks)
    {
        foreach (var attack in attacks)
        {
            var collisions = _collisionDetector.CheckCollides(
                attack.Weapon,
                new Move(attack.Weapon.AttackDirection, attack.Weapon.Position),
                GetEnemies(attack.Attacker));

            foreach (var collision in collisions)
            {
                var target = (Entity)collision.AnotherEntity;
                var damage = CalculateDamage(attack, collision);
                ApplyDamageIfNewAttack(attack, target, damage);
                if (target.Health.IsDead)
                {
                    target.Die();
                    _state.RemoveGameEntity(target);
                }
            }
        }
    }

    private void ApplyDamageIfNewAttack(AttackInfo attack, Entity target, int damage)
    {
        var attackedAnyone = _attackedEntitiesByWeapon.TryGetValue(attack.AttackId, out var damagedEnemies);
        if (attackedAnyone)
        {
            if (damagedEnemies.Contains(target.Id))
            {
                return;
            }
        }

        target.TakeDamage(damage);

        if (attackedAnyone)
        {
            damagedEnemies.Add(target.Id);
        }
        else
        {
            _attackedEntitiesByWeapon.Add(attack.AttackId, new HashSet<Guid> { target.Id });
        }
    }

    private IReadOnlyList<object> GetEnemies(Entity attacker)
    {
        return _state.AllGameEntities
            .Where(e => e is Entity entity && attacker.IsEnemy(entity))
            .ToList();
    }

    private int CalculateDamage(AttackInfo attack, Collision collision)
    {
        return (int)Math.Round(attack.Weapon.Damage);
    }

    public void Dispose()
    {
        _state.OnMarkingAttack -= OnWeaponActivation;
    }

    ~AttackingEngine()
    {
        Dispose();
    }
}

public class AttackInfo
{
    public Guid AttackId { get; }
    public Entity Attacker { get; }
    public Weapon Weapon => Attacker.Weapon;
    public Direction AttackDirection => Weapon.AttackDirection;
    public Position OldPosition { get; }
    public Position NewPosition { get; }

    public AttackInfo(Guid id, Entity attacker, Position oldPosition, Position newPosition)
    {
        AttackId = id;
        Attacker = attacker;
        OldPosition = oldPosition;
        NewPosition = newPosition;
    }
}
