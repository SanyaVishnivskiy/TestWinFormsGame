namespace TestGame.UI.Game.Moving;

internal class PlayerMovingStrategy : IWalkable
{
    private readonly HashSet<MoveDirection> _activeMovings = new(4);
    private readonly HashSet<MoveDirection> _deniedMovings = new(4);

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

        Logger.Log($"Active Directions: {string.Join(", ", _activeMovings)}, denied: {string.Join(", ", _deniedMovings)}");

        _deniedMovings.Clear();
    }

    private void MovePosition(Position position)
    {
        var allowedMovings = _activeMovings.Except(_deniedMovings);

        if (allowedMovings.Contains(MoveDirection.Left))
        {
            position.AddX(-Moving.Speed);
        }

        if (allowedMovings.Contains(MoveDirection.Right))
        {
            position.AddX(Moving.Speed);
        }

        if (allowedMovings.Contains(MoveDirection.Up))
        {
            position.AddY(-Moving.Speed);
        }

        if (allowedMovings.Contains(MoveDirection.Down))
        {
            position.AddY(Moving.Speed);
        }
    }

    public void DenyMoveToDirectionOnce(MoveDirection direction)
    {
        _deniedMovings.Add(direction);
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
