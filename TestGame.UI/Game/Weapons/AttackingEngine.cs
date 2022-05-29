namespace TestGame.UI.Game.Weapons;

public class AttackingEngine : IDisposable
{
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

            entity.AttackTick();

            if (entity.Weapon.Attacking)
            {
                var currentPosition = entity.CurrentPosition.Clone();
                attacks.Add(new AttackInfo(entity, entity.Weapon.AttackDirection, previousPosition, currentPosition));
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
                target.TakeDamage(damage);
            }
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
    public Entity Attacker { get; }
    public Weapon Weapon => Attacker.Weapon;
    public Direction AttackDirection { get; }
    public Position OldPosition { get; }
    public Position NewPosition { get; }

    public AttackInfo(Entity attacker, Direction direction, Position oldPosition, Position newPosition)
    {
        Attacker = attacker;
        AttackDirection = direction;
        OldPosition = oldPosition;
        NewPosition = newPosition;
    }
}
