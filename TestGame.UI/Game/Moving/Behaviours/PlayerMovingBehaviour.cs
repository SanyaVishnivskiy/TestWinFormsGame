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
        if (_activeMovings.Contains(MoveDirection.Left))
        {
            position.AddX(GetMoveDistance(MoveDirection.Left, -1));
        }

        if (_activeMovings.Contains(MoveDirection.Right))
        {
            position.AddX(GetMoveDistance(MoveDirection.Right, 1));
        }

        if (_activeMovings.Contains(MoveDirection.Up))
        {
            position.AddY(GetMoveDistance(MoveDirection.Up, -1));
        }

        if (_activeMovings.Contains(MoveDirection.Down))
        {
            position.AddY(GetMoveDistance(MoveDirection.Down, 1));
        }

        var direction = new Direction();
        foreach (var moving in _activeMovings)
        {
            direction.Add(moving);
        }

        return new Move(direction, position.Clone());
    }

    private float GetMoveDistance(MoveDirection direction, int sign)
    {
        if (_adjustedMovings.TryGetValue(direction, out var distance))
        {
            return distance * sign;
        }

        return MoveDistanceCalculator.Calculate(Moving.Speed * sign);
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
