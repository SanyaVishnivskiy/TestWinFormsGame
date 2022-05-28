namespace TestGame.UI.Game.World;

public interface IGameMap
{
    string Name { get; }
    int Width { get; }
    int Height { get; }
    Point PlayerSpawnTile { get; }

    IEnumerable<GroundTile> GetGroundTiles();
    IEnumerable<MapObject> GetMapObjects();
    GroundTile GetGroundTile(int x, int y);
    bool CheckValidSpawnWithWater(Point point);
    Point CalculateTilePositionWithWaterInTiles(Point point);
    Size GetSizeInPixels();
}

internal class GameMap : IGameMap
{
    public string Name { get; set; }

    public int Width => _groundLayer.GetLength(0);
    public int Height => _groundLayer.GetLength(1);
    public int WidthWithWater => GroundLayer.GetLength(1);
    public int HeightWithWater => GroundLayer.GetLength(0);

    private int[,] _groundLayer { get; set; }
    private int[,] _objectLayer { get; set; }

    public GroundTile[,] GroundLayer { get; }
    public MapObject?[,] ObjectsLayer { get; }
    public Point PlayerSpawnTile { get; }

    private TilesFactory _tilesFactory;

    public GameMap(
        string name,
        int[,] groundLayer,
        Dictionary<Point, MapObjectType> objectLayer,
        Point playerSpawn)
        : this(
              name,
              groundLayer,
              ConvertObjectsListToMapView(objectLayer, groundLayer.GetLength(0), groundLayer.GetLength(1)),
              playerSpawn)
    {
    }

    public GameMap(
        string name,
        int[,] groundLayer,
        int[,] objectLayer,
        Point playerSpawn)
    {
        Name = name;
        if (groundLayer.GetLength(0) != objectLayer.GetLength(0) || groundLayer.GetLength(1) != objectLayer.GetLength(1))
            throw new ArgumentException("Ground and object layers must have the same size");

        _tilesFactory = new TilesFactory();

        _groundLayer = groundLayer;
        _objectLayer = objectLayer;

        GroundLayer = new GroundTile[
            CalculateLengthWithWater(Width),
            CalculateLengthWithWater(Height)];
        ObjectsLayer = new MapObject[
            CalculateLengthWithWater(Width),
            CalculateLengthWithWater(Height)];
        InitMap();

        PlayerSpawnTile = playerSpawn;
    }

    private Position ConvertTilesToPixels(Point tile) => TileToPositionConverter.ConvertTilesToPosition(tile);

    private void InitMap()
    {
        var groundLayerWithWater = ExpandLayerWithWaterBorders(_groundLayer);
        groundLayerWithWater.Loop((row, column) =>
        {
            GroundLayer[row, column] = _tilesFactory.CreateGroundTile(
                    ConvertTilesToPixels(new Point(column, row)),
                    (TileType)groundLayerWithWater[row, column]);
        });

        var objectLayerWithWater = ExpandLayerWithWaterBorders(_objectLayer);
        objectLayerWithWater.Loop((row, column) =>
        {
            ObjectsLayer[row, column] = _tilesFactory.CreateMapObject(
                    ConvertTilesToPixels(new Point(column, row)),
                    (MapObjectType)objectLayerWithWater[row, column]);
        });
    }

    private int[,] ExpandLayerWithWaterBorders(int[,] layer)
    {
        var layerWithWater = new int[
            CalculateLengthWithWater(Width),
            CalculateLengthWithWater(Height)];

        layerWithWater.Fill((int)TileType.Water);

        layer.Loop((row, column) =>
        {
            var originalPosition = CalculateTilePositionWithWaterInTiles(new Point(row, column));
            layerWithWater[originalPosition.X, originalPosition.Y] = layer[row, column];
        });

        return layerWithWater;
    }

    private int CalculateLengthWithWater(int mapLength) => mapLength + 2 * Constants.WaterBorderSizeInTiles;

    public Point CalculateTilePositionWithWaterInTiles(Point point) =>
        new Point(point.X + Constants.WaterBorderSizeInTiles, point.Y + Constants.WaterBorderSizeInTiles);

    public IEnumerable<GroundTile> GetGroundTiles()
    {
        return GroundLayer.Traverse();
    }
    
    public IEnumerable<MapObject> GetMapObjects()
    {
        return ObjectsLayer.TraverseWithFilter((row, column) => ObjectsLayer[row, column] is not null)!;
    }

    private static int[,] ConvertObjectsListToMapView(Dictionary<Point, MapObjectType> objectLayer, int width, int height)
    {
        var objectLayerView = new int[width, height];

        foreach (var obj in objectLayer)
        {
            objectLayerView[obj.Key.Y, obj.Key.X] = (int)obj.Value;
        }

        return objectLayerView;
    }

    public GroundTile GetGroundTile(int x, int y)
    {
        return GroundLayer[x, y];
    }

    public bool CheckValidSpawnWithWater(Point point)
    {
        if (!GroundLayer[point.Y, point.X].SpawnAllowed)
        {
            return false;
        }

        if (ObjectsLayer[point.Y, point.X] is not null)
        {
            return false;
        }

        return true;
    }

    public Size GetSizeInPixels()
    {
        return new Size(
            WidthWithWater * Constants.TileWidth,
            HeightWithWater * Constants.TileHeight);
    }
}
