namespace TestGame.UI.Game.Moving;

public class Direction
{
    private HashSet<MoveDirection> _directions = new(2);
    public IReadOnlySet<MoveDirection> Directions => _directions;

    public MoveDirection Horizontal => Directions.SingleOrDefault(x => x == MoveDirection.Left || x == MoveDirection.Right);
    public MoveDirection Vertical => Directions.SingleOrDefault(x => x == MoveDirection.Up || x == MoveDirection.Down);
    public bool IsHorizontal => Horizontal != MoveDirection.None;
    public bool IsVertical => Vertical != MoveDirection.None;
    public bool IsDiagonal => IsHorizontal && IsVertical;

    public bool IsEmpty => _directions.Count == 0;

    public Direction()
    {
        _directions = new HashSet<MoveDirection>();
    }

    public Direction(MoveDirection direction) : this()
    {
        Add(direction);
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

    public override bool Equals(object? obj)
    {
        if (this is null && obj is null)
        {
            return true;
        }

        if (this is null || obj is null)
        {
            return false;
        }

        if (obj is Direction direction)
        {
            return _directions.SetEquals(direction._directions);
        }

        return false;
    }

    public override int GetHashCode()
    {
        if (this is null)
        {
            return 0;
        }

        return _directions.GetHashCode();
    }
}
