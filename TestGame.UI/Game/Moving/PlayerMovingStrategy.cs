namespace TestGame.UI.Game.Moving;

internal class PlayerMovingStrategy : IWalkable
{
    private readonly HashSet<MoveDirection> _activeMovings = new(4);
    private readonly Dictionary<MoveDirection, float> _adjustedMovings = new(4);

    public Position CurrentPosition { get; }
    public MovingInfo Moving { get; }

    public PlayerMovingStrategy(Position currentPosition, MovingInfo movingInfo)
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

    public Position GetNewMove()
    {
        var position = CurrentPosition.Clone();
        MovePosition(position);
        return position;
    }

    public void Move()
    {
        MovePosition(CurrentPosition);

        //Logger.Log($"Active Directions: {string.Join(", ", _activeMovings)}, denied: {string.Join(", ", _deniedMovings)}");

        _adjustedMovings.Clear();
    }

    private void MovePosition(Position position)
    {
        var movings = _activeMovings
            .Select(x => new MoveAdjustment(
                x,
                _adjustedMovings.TryGetValue(x, out var distance)
                    ? distance
                    : Moving.Speed))
            .ToDictionary(x => x.MoveDirection, x => x.MaxDistance);

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
    }

    public void AdjustMovementOnce(MoveAdjustment direction)
    {
        if (_adjustedMovings.TryGetValue(direction.MoveDirection, out var distance))
        {
            _adjustedMovings[direction.MoveDirection] = Math.Max(direction.MaxDistance, distance);
        }
        else
        {
            _adjustedMovings.Add(direction.MoveDirection, direction.MaxDistance);
        }
    }
}

public class MovingInfo
{
    private float _speed;
    public float Speed
    {
        get => _speed * Constants.GameSpeed;
        set => _speed = value;
    }
}
