namespace TestGame.UI.Game.Moving
{
    public class DirectionCalculator
    {
        public static Direction CalculateDirection(Position first, Position second)
        {
            var direction = new Direction();
            direction.Add(CalculateHorizontalDirection(first, second));
            direction.Add(CalculateVerticalDirection(first, second));
            return direction;
        }
        
        public static MoveDirection CalculateHorizontalDirection(Position first, Position second)
        {
            if (first.X > second.X)
            {
                return MoveDirection.Left;
            }

            if (first.X < second.X)
            {
                return MoveDirection.Right;
            }

            return MoveDirection.None;
        }

        public static MoveDirection CalculateVerticalDirection(Position first, Position second)
        {
            if (first.Y > second.Y)
            {
                return MoveDirection.Up;
            }

            if (first.Y < second.Y)
            {
                return MoveDirection.Down;
            }

            return MoveDirection.None;
        }
    }
}
