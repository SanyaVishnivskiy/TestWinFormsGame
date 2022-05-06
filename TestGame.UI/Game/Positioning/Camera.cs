namespace TestGame.UI.Game.Positioning
{
    internal class Camera
    {
        private readonly Player _entity;
        
        public Camera(Player entity, Size clientSize)
        {
            _entity = entity;
            ClientSize = clientSize;
        }

        public Position Position => _entity.Position;
        public Position CentralPosition => new Position(
            ClientSize.Width / 2 - Position.X - _entity.Animation.CurrentFrame.Width / 2,
            ClientSize.Height / 2 - Position.Y - _entity.Animation.CurrentFrame.Height / 2);
        public Size ClientSize { get; set; }

        public Position ToCameraPosition(Position position)
        {
            var newX = position.X + CentralPosition.X;
            var newY = position.Y + CentralPosition.Y;
            return new Position(newX, newY);
        }
    }
}
