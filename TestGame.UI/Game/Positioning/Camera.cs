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
            ClientSize.Width / 2 - ZoomX(Position.X) - ZoomX(_entity.Animation.CurrentFrame.Width) / 2,
            ClientSize.Height / 2 - ZoomY(Position.Y) - ZoomY(_entity.Animation.CurrentFrame.Height) / 2);
        public Size ClientSize { get; set; }

        public Position ToCameraPosition(Position position)
        {
            var x = ZoomX(position.X) + CentralPosition.X;
            var y = ZoomY(position.Y) + CentralPosition.Y;
            return new Position(x, y);
        }

        private float ZoomX(float value) => ViewResizer.CalculateWidthProportion(value);
        private float ZoomY(float value) => ViewResizer.CalculateHeightProportion(value);
    }
}
