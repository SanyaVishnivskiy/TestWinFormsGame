namespace TestGame.UI.Game.World
{
    public abstract class Tile : IRenderable
    {
        public Tile(Position position)
        {
            Position = position;
        }

        public Position Position { get; }
        public abstract TileType Type { get; }
        public abstract Animation Animation { get; }

        public Bitmap GetTexture()
        {
            var texture = Animation.GetNextFrame();
            return new Bitmap(texture, new Size(Constants.TileWidth, Constants.TileHeight));
        }

        public override string ToString()
        {
            return $"{Position} {Type}";
        }
    }
}
