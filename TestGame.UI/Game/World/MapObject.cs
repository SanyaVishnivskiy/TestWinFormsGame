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
