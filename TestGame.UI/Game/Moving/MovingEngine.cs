namespace TestGame.UI.Game.Moving;

public class MovingEngine : IDisposable
{
    private IReadOnlyList<IMovable> _movableEntities;

    private CollisionDetector _collisionDetector;

    public MovingEngine()
    {
        GameState.Instance.OnAllGameEntitiesChange += OnListUpdate;
        OnListUpdate(this, new GameEntitiesChangeEventArgs(GameState.Instance.AllGameEntities));

        _collisionDetector = new CollisionDetector(GameState.Instance.Map);
    }

    private void OnListUpdate(object? sender, GameEntitiesChangeEventArgs args)
    {
        _movableEntities = args.Entities.OfType<IMovable>().ToList();
    }

    public void Move()
    {
        foreach (var movable in _movableEntities)
        {
            if (movable is null)
            {
                continue;
            }

            if (movable is ICollidable collidable)
            {
                DenyBlockedDirections(movable, collidable);
            }

            movable.Move();
        }
    }

    private void DenyBlockedDirections(IMovable movable, ICollidable collidable)
    {
        var position = movable.GetNewMove();
        if (position.X == collidable.Hitbox.X && position.Y == collidable.Hitbox.Y)
        {
            return;
        }

        var collisions = _collisionDetector.CalculateCollisionsWithMap(collidable, position);
        var deniedDirections = ConvertCollisionsToDeniedDirections(collisions);
        foreach (var directionToDeny in deniedDirections)
        {
            movable.DenyMoveToDirectionOnce(directionToDeny);
        }
    }

    private HashSet<MoveDirection> ConvertCollisionsToDeniedDirections(List<Collision> collisions)
    {
        if (collisions.Count == 0)
        {
            return new HashSet<MoveDirection>();
        }

        return collisions.SelectMany(c => c.Direction.Directions).ToHashSet();
    }

    public void Dispose()
    {
        GameState.Instance.OnAllGameEntitiesChange -= OnListUpdate;
    }

    ~MovingEngine()
    {
        Dispose();
    }
}
