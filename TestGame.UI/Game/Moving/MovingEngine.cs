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

            var newPosition = movable.GetNewMove();
            if (movable.CurrentPosition.Equals(newPosition))
            {
                return;
            }

            if (movable is ICollidable collidable)
            {
                DenyBlockedDirections(movable, collidable, newPosition);
            }

            movable.Move();
        }
    }

    private void DenyBlockedDirections(IMovable movable, ICollidable collidable, Position newPosition)
    {
        var collisions = _collisionDetector.CalculateCollisionsWithMap(collidable, movable.GetNewMove());
        var deniedDirections = ConvertCollisionsToDeniedDirections(
            collisions,
            WasDiagonalMove(movable.CurrentPosition, newPosition));
        foreach (var directionToDeny in deniedDirections)
        {
            movable.DenyMoveToDirectionOnce(directionToDeny);
        }
    }

    private bool WasDiagonalMove(Position currentPosition, Position newPosition)
    {
        return currentPosition.X != newPosition.X && currentPosition.Y != newPosition.Y;
    }

    private HashSet<MoveDirection> ConvertCollisionsToDeniedDirections(List<Collision> collisions, bool wasDiagonalMove)
    {
        if (collisions.Count == 0)
        {
            return new HashSet<MoveDirection>();
        }

        var uniqueDirections = collisions.Select(c => c.Direction).Distinct();
        if (!wasDiagonalMove)
        {
            return uniqueDirections
                .Where(d => !d.IsDiagonal)
                .SelectMany(x => x.Directions)
                .ToHashSet();
        }

        var diagonalDirection = uniqueDirections.FirstOrDefault(d => d.IsDiagonal);
        if (diagonalDirection is null)
        {
            return new HashSet<MoveDirection>();
        }

        if (uniqueDirections.Count() == 1)
        {
            // horizontal direction has more priority, so chosing between horizontal and diagonal we 
            // will always choose horizontal. To achive it we should block vertical movement if
            // result has only diagonal move
            return new HashSet<MoveDirection> { diagonalDirection.Vertical };
        }

        var result = new HashSet<MoveDirection>();
        var nonDiagonalDirections = uniqueDirections.Where(d => !d.IsDiagonal);

        var verticalDirection = nonDiagonalDirections.FirstOrDefault(d => d.IsVertical);
        if (verticalDirection is not null)
        {
            result.Add(verticalDirection.Vertical);
        }

        var horizontalDirection = nonDiagonalDirections.FirstOrDefault(d => d.IsHorizontal);
        if (horizontalDirection is not null)
        {
            result.Add(horizontalDirection.Horizontal);
        }

        return result;
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
