namespace TestGame.UI.Game.Moving;

public class MovingEngine : IDisposable
{
    private IReadOnlyList<IMovable> _movableEntities;

    private CollisionDetector _collisionDetector;

    public MovingEngine(GameState state)
    {
        state.OnAllGameEntitiesChange += OnListUpdate;
        OnListUpdate(this, new GameEntitiesChangeEventArgs(state.AllGameEntities));

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

            var move = movable.GetNewMove();
            if (movable.CurrentPosition.Equals(move.Position))
            {
                return;
            }

            if (movable is ICollidable collidable)
            {
                var adjustedMoves = AdjustBlockedDirections(collidable, move);
                foreach (var adjustedMove in adjustedMoves)
                {
                    movable.AdjustMovementOnce(adjustedMove);
                }
            }

            movable.Move();
        }
    }

    private List<MoveAdjustment> AdjustBlockedDirections(
        ICollidable collidable,
        Move move)
    {
        var collisions = _collisionDetector.CalculateCollisions(collidable, move);
        var deniedDirections = ConvertCollisionsToDeniedDirections(
            collisions,
            move.Direction.IsDiagonal);
        return AdjustMoves(collisions, deniedDirections);
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

    private List<MoveAdjustment> AdjustMoves(List<Collision> collisions, HashSet<MoveDirection> deniedDirections)
    {
        var oneDirectionCollision = collisions
            .SelectMany(x =>
            {
                if (x.Direction.IsDiagonal)
                {
                    return new[] {
                        new Collision(x.Entity, x.AnotherEntity, new Direction(x.Direction.Horizontal)),
                        new Collision(x.Entity, x.AnotherEntity, new Direction(x.Direction.Vertical))
                    };
                }
                return new[] { x };
            });

        var distancesByDirection = CalculateDistancesByDirection(
            oneDirectionCollision.Where(x => x.Direction.Directions.Any(d => deniedDirections.Contains(d))));

        return distancesByDirection
            .Select(x => new MoveAdjustment(x.Key, x.Value.Max()))
            .ToList();
    }

    private Dictionary<MoveDirection, HashSet<float>> CalculateDistancesByDirection(IEnumerable<Collision> collisions)
    {
        return collisions
            .SelectMany(x => x.Direction.Directions
                .Select(d => new {
                    Direction = d,
                    Distance = CalculateDistanceToObstacle(d, x.Entity, x.AnotherEntity)
                }))
            .GroupBy(x => x.Direction)
            .ToDictionary(x => x.Key, x => x.Select(a => a.Distance).ToHashSet());
    }

    private float CalculateDistanceToObstacle(MoveDirection direction, ICollidable collidable, ICollidable anotherCollidable)
    {
        return DirectionCalculator.CalculateDistanceToObstacle(direction, collidable.Hitbox, anotherCollidable.Hitbox);
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
