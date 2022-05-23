namespace TestGame.UI.Game.Moving.Behaviours
{
    public class NoMovingBehaviour : IMovable
    {
        public NoMovingBehaviour(Position position)
        {
            CurrentPosition = position;
        }

        public Position CurrentPosition { get; }

        public void AdjustMovementOnce(MoveAdjustment direction)
        {
        }

        public Move GetNewMove()
        {
            return new Move(Direction.None, CurrentPosition);
        }

        public Move Move()
        {
            return new Move(Direction.None, CurrentPosition);
        }
    }
}
