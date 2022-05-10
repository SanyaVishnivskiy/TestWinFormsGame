namespace TestGame.UI.Game.World;

public static class TileToPositionConverter
{
    public static Position ConvertTilesToPosition(Point tile)
    {
        return new Position(tile.X * Constants.TileWidth, tile.Y * Constants.TileHeight);
    }

    public static Point ToTile(Position position)
    {
        var x = Math.Floor((double)(position.X / Constants.TileWidth));
        var y = Math.Floor((double)(position.Y / Constants.TileHeight));
        return new Point((int)x, (int)y);
    }

    public static int GetTilesCount(float dimensionStartPositio, float distance, int tileLength)
    {
        var startPositionOnFirstTile = dimensionStartPositio % tileLength;
        var tilesCount = (startPositionOnFirstTile + distance) / tileLength;
        var count = (int)Math.Ceiling((double)tilesCount);
        // if exactly on 0,0 coordinate of the tile above calculation will return 0
        // but actually we are on the tile
        if (count == 0)
        {
            count = 1;
        }
        return count;
    }
}
