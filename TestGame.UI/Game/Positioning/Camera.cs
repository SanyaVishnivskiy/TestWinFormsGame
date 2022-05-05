namespace TestGame.UI.Game.Positioning
{
    internal class Camera
    {
        public Camera(Player entity, Size clientSize)
        {
            Position = entity.Position;
            ClientSize = clientSize;
        }

        public Position Position { get; }
        public Position CentralPosition => new Position(ClientSize.Width / 2 - Position.X, ClientSize.Height / 2 - Position.Y);
        public Size ClientSize { get; }

        public Position ToCameraPosition(Position position)
        {
            var newX = position.X + CentralPosition.X;
            var newY = position.Y + CentralPosition.Y;
            return new Position(newX, newY);
        }
    }
}
