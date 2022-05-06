namespace TestGame.UI.Game.World
{
    public class Tile : IRenderable
    {
        public Tile(Position position, GroundTileType type)
        {
            Position = position;
            Type = type;
        }

        public Position Position { get; }
        public GroundTileType Type { get; }

        public virtual Bitmap GetFrame()
        {
            var texture = TileTypeTextureMap.GetTexture(Type);
            var result = new Bitmap(texture, new Size(Constants.TileWidth, Constants.TileHeight));
            return result;
        }

        public override string ToString()
        {
            return $"{Position} {Type}";
        }
    }
}
