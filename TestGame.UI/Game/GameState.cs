namespace TestGame.UI.Game;

public class GameState
{
    public static readonly GameState Instance = new GameState();

    private List<object> _allGameEntities = new();
    public IReadOnlyList<object> AllGameEntities => _allGameEntities;
    public event EventHandler<GameEntitiesChangeEventArgs> OnAllGameEntitiesChange;

    private List<Entity> _attackingEntities = new();
    public IReadOnlyList<Entity> AttackingEntities => _attackingEntities;
    public event EventHandler<WeaponActivationEventArgs> OnMarkingAttack;

    public Player Player { get; set; }
    public Camera Camera { get; private set; }
    public IGameMap Map { get; set; }

    public void Init(Size clientSize)
    {
        if (Player is null)
        {
            throw new Exception("Player is not spawned");
        }

        if (Map is null)
        {
            throw new Exception("Map is not set");
        }

        Camera = new Camera(Player, clientSize);
    }

    public void AddGameEntity(object entity)
    {
        _allGameEntities.Add(entity);
        OnAllGameEntitiesChange?.Invoke(this, new GameEntitiesChangeEventArgs(_allGameEntities));
    }

    public void MarkAttacking(Entity entity)
    {
        _attackingEntities.Add(entity);
        OnMarkingAttack?.Invoke(this, new WeaponActivationEventArgs(_attackingEntities));
    }

    public void MarkAttackFinished(Entity entity)
    {
        _attackingEntities.Remove(entity);
        OnMarkingAttack?.Invoke(this, new WeaponActivationEventArgs(_attackingEntities));
    }
}

public class WeaponActivationEventArgs : EventArgs
{
    public WeaponActivationEventArgs(IReadOnlyList<Entity> weapons)
    {
        Weapons = weapons;
    }

    public IReadOnlyList<Entity> Weapons { get; }
}

public class GameEntitiesChangeEventArgs : EventArgs
{
    public GameEntitiesChangeEventArgs(IReadOnlyList<object> entities)
    {
        Entities = entities;
    }

    public IReadOnlyList<object> Entities { get; }
}
