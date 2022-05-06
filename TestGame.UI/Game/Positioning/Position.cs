namespace TestGame.UI.Game.Positioning
{
    public class Position
    {
        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; private set; }
        public float Y { get; private set; }

        public void AddX(float distance)
        {
            X += distance;
        }

        public void AddY(float distance)
        {
            Y += distance;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null && this is null)
            {
                return true;
            }

            if (obj is null || this is null)
            {
                return false;
            }

            if (obj is Position position)
            {
                return X == position.X && Y == position.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (this is null)
            {
                return 0;
            }

            return (int)X * 346 << (int)Y * 346;
        }

        public override string ToString()
        {
            return $"X: {X} Y: {Y}";
        }
    }
}
