namespace TestGame.UI.Game.World
{
    public abstract class MapObject : IRenderable
    {
        public MapObject(Position position)
        {
            Position = position;
            Width = Animation.FirstFrame.Width;
            Height = Animation.FirstFrame.Height;
        }

        public Position Position { get; }
        public abstract MapObjectType Type { get; }
        public abstract Animation Animation { get; }

        public virtual int Width { get; }
        public virtual int Height { get; }

        public Bitmap GetTexture()
        {
            return Animation.GetNextFrame();
        }
    }

    public enum MapObjectType
    {
        None = 0,
    }
}
