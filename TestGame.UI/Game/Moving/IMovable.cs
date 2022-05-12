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
        Position GetNewMove();
        void AdjustMovementOnce(MoveAdjustment direction);
        void Move();
    }

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
    }
}
