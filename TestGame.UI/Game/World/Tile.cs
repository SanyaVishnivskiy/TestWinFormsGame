namespace TestGame.UI.Game.World
{
    public abstract class Tile : IRenderable
    {
        public Tile(Position position)
        {
            Position = position;
        }

        public virtual int Width => Constants.TileWidth;
        public virtual int Height => Constants.TileHeight;

        public Position Position { get; }
        public abstract TileType Type { get; }
        public abstract Animation Animation { get; }

        public bool SpawnAllowed { get; protected set; } = true;

        public Bitmap GetTexture()
        {
            return Animation.GetNextFrame();
        }

        public override string ToString()
        {
            return $"{Position} {Type}";
        }
    }
}
