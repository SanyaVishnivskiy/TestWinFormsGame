namespace TestGame.UI.Game.World
{
    public abstract class MapObject : IRenderable
    {
        public MapObject(Position position)
        {
            Position = position;
        }

        public Position Position { get; }
        public abstract MapObjectType Type { get; }
        public abstract Animation Animation { get; }

        public virtual int Width => Animation.FirstFrame.Width;
        public virtual int Height => Animation.FirstFrame.Height;
        public Bitmap CurrentTexture => Animation.CurrentFrame;

        public Bitmap GetTexture()
        {
            return Animation.GetNextFrame();
        }

        public override string ToString()
        {
            return $"{Position} {Type}";
        }
    }

    public enum MapObjectType
    {
        None = 0,
        Tree = 1,
    }
}
