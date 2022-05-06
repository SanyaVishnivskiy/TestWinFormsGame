namespace TestGame.UI.Game.World
{
    public class GroundTile : Tile
    {
        public GroundTile(Position position, GroundTileType type) : base(position, type)
        {
        }
    }

    public enum GroundTileType
    {
        Dirt = 1,
        Grass = 2,
    }
    
    public static class TileTypeTextureMap
    {
        public static Dictionary<GroundTileType, Bitmap> _tileTypeTextureMap = new()
        {
            { GroundTileType.Dirt, Textures.Dirt },
            { GroundTileType.Grass, Textures.Grass },
        };
        
        public static Bitmap GetTexture(GroundTileType type)
        {
            return _tileTypeTextureMap[type];
        }
    }
}
