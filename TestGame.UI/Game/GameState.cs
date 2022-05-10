namespace TestGame.UI.Game;

internal class GameState
{
    public static readonly GameState Instance = new GameState();

    private List<object> _allGameEntities = new List<object>();

    public IReadOnlyList<object> AllGameEntities => _allGameEntities;
    public event EventHandler<GameEntitiesChangeEventArgs> OnAllGameEntitiesChange;

    public Player Player { get; private set; }
    public Camera Camera { get; private set; }
    public IGameMap Map { get; private set; }

    public void Init(Size clientSize)
    {
        var mapsProvider = new InMemoryMapsProvider();
        Map = mapsProvider.GetMapByName("Default");

        Player = new Player(Map.PlayerSpawnPosition);
        Camera = new Camera(Player, clientSize);

        _allGameEntities.Add(Player);
        //Player.StartMoving(MoveDirection.Right);
        //Player.StartMoving(MoveDirection.Up);
    }

    public void AddGameEntity(object entity)
    {
        _allGameEntities.Add(entity);
        OnAllGameEntitiesChange?.Invoke(this, new GameEntitiesChangeEventArgs(_allGameEntities));
    }
}

public class GameEntitiesChangeEventArgs : EventArgs
{
    public GameEntitiesChangeEventArgs(IReadOnlyList<object> entities)
    {
        Entities = entities;
    }

    public IReadOnlyList<object> Entities { get; }
}
