namespace TestGame.UI.Game.Moving;

internal class PlayerMovingBehaviour : IWalkable
{
    private readonly HashSet<MoveDirection> _activeMovings = new(4);
    private readonly Dictionary<MoveDirection, float> _adjustedMovings = new(4);

    public Position CurrentPosition { get; }
    public MovingInfo Moving { get; }

    public PlayerMovingBehaviour(Position currentPosition, MovingInfo movingInfo)
    {
        CurrentPosition = currentPosition;
        Moving = movingInfo;
    }

    public void FinishMoving(MoveDirection direction)
    {
        _activeMovings.Remove(direction);
    }

    public void StartMoving(MoveDirection direction)
    {
        _activeMovings.Add(direction);
    }

    public Move GetNewMove()
    {
        var position = CurrentPosition.Clone();
        return MovePosition(position);
    }

    public Move Move()
    {
        var move = MovePosition(CurrentPosition);

        Logger.Log($"Active Directions: {string.Join(", ", _activeMovings)}," +
            $" adjusted: {string.Join(", ", _adjustedMovings)}");

        _adjustedMovings.Clear();

        return move;
    }

    private Move MovePosition(Position position)
    {
        var movings = GetDirectionsAndDistance();

        float distance;
        if (movings.TryGetValue(MoveDirection.Left, out distance))
        {
            position.AddX(-distance);
        }

        if (movings.TryGetValue(MoveDirection.Right, out distance))
        {
            position.AddX(distance);
        }

        if (movings.TryGetValue(MoveDirection.Up, out distance))
        {
            position.AddY(-distance);
        }

        if (movings.TryGetValue(MoveDirection.Down, out distance))
        {
            position.AddY(distance);
        }

        var direction = new Direction();
        foreach (var moving in movings.Keys)
        {
            direction.Add(moving);
        }

        return new Move(direction, position.Clone());
    }

    private Dictionary<MoveDirection, float> GetDirectionsAndDistance()
    {
        return _activeMovings
               .Select(x => new MoveAdjustment(
                   x,
                   _adjustedMovings.TryGetValue(x, out var distance)
                       ? distance
                       : Moving.Speed))
               .ToDictionary(x => x.MoveDirection, x => x.MaxDistance);
    }

    public void AdjustMovementOnce(MoveAdjustment direction)
    {
        if (_adjustedMovings.TryGetValue(direction.MoveDirection, out var distance))
        {
            _adjustedMovings[direction.MoveDirection] = Math.Min(direction.MaxDistance, distance);
        }
        else
        {
            _adjustedMovings.Add(direction.MoveDirection, direction.MaxDistance);
        }
    }
}
