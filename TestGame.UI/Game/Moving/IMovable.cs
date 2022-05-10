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
        Position GetNewMove();
        void DenyMoveToDirectionOnce(MoveDirection direction);
        void Move();
    }

    public interface IWalkable : IMovable
    {
        void StartMoving(MoveDirection direction);
        void FinishMoving(MoveDirection direction);
    }
}
