namespace TestGame.UI.Game.Moving;

public class Direction
{
    private HashSet<MoveDirection> _directions = new(2);
    public IReadOnlySet<MoveDirection> Directions => _directions;
    public MoveDirection Horizontal => Directions.SingleOrDefault(x => x == MoveDirection.Left || x == MoveDirection.Right);
    public MoveDirection Vertical => Directions.SingleOrDefault(x => x == MoveDirection.Up || x == MoveDirection.Down);

    public Direction()
    {
        _directions = new HashSet<MoveDirection>();
    }

    public void Add(MoveDirection direction)
    {
        if (direction == MoveDirection.None)
        {
            return;
        }

        if (_directions.Contains(direction))
        {
            return;
        }

        if (_directions.Contains(GetOppositeDirection(direction)))
        {
            _directions.Remove(GetOppositeDirection(direction));
            return;
        }

        _directions.Add(direction);
    }

    internal void Remove(MoveDirection direction)
    {
        _directions.Remove(direction);
    }

    public static MoveDirection GetOppositeDirection(MoveDirection direction)
    {
        if (direction == MoveDirection.Up)
        {
            return MoveDirection.Down;
        }
        if (direction == MoveDirection.Down)
        {
            return MoveDirection.Up;
        }
        if (direction == MoveDirection.Left)
        {
            return MoveDirection.Right;
        }
        if (direction == MoveDirection.Right)
        {
            return MoveDirection.Left;
        }

        return MoveDirection.None;
    }
}
