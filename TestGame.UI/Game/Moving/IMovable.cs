namespace TestGame.UI.Game.Moving
{
    public enum MoveDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public interface IMovable
    {
        Position CurrentPosition { get; }
        Move GetNewMove();
        void AdjustMovementOnce(MoveAdjustment direction);
        Move Move();
    }

    public record Move(Direction Direction, Position Position);

    public interface IWalkable : IMovable
    {
        void StartMoving(MoveDirection direction);
        void FinishMoving(MoveDirection direction);
    }

    public class MoveAdjustment
    {
        public MoveDirection MoveDirection { get; }
        public float MaxDistance { get; }

        public MoveAdjustment(MoveDirection direction, float maxDistance)
        {
            MoveDirection = direction;
            MaxDistance = maxDistance;
        }

        public override string ToString()
        {
            return $"{MoveDirection} {MaxDistance}";
        }
    }
}
