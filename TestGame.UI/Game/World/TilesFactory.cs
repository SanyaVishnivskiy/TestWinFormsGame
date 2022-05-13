using TestGame.UI.Game.World.MapObjects;
using TestGame.UI.Game.World.Tiles;

namespace TestGame.UI.Game.World;

public class TilesFactory
{
    public GroundTile CreateGroundTile(Position position, TileType type)
    {
        return type switch
        {
            TileType.Dirt => new DirtTile(position),
            TileType.Grass => new GrassTile(position),
            TileType.Water => new WaterTile(position),
            _ => throw new ArgumentException("Unknown tile type")
        };
    }

    public MapObject? CreateMapObject(Position position, MapObjectType type)
    {
        return type switch
        {
           MapObjectType.Tree => new Tree(position),
           _ => null
        };
    }
}
