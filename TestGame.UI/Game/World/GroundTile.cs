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
        Water = 0,
        Dirt = 1,
        Grass = 2,
    }
    
    public static class TileTypeTextureMap
    {
        public static Dictionary<GroundTileType, Bitmap> _tileTypeTextureMap = new()
        {
            { GroundTileType.Dirt, Textures.Dirt },
            { GroundTileType.Grass, Textures.Grass },
            { GroundTileType.Water, Textures.Water },
        };
        
        public static Bitmap GetTexture(GroundTileType type)
        {
            return _tileTypeTextureMap[type];
        }
    }
}
