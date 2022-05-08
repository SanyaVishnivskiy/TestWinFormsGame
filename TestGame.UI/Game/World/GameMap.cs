namespace TestGame.UI.Game.World
{
    public interface IGameMap
    {
        string Name { get; }
        int Width { get; }
        int Height { get; }
        Point PlayerSpawnInTiles { get; }
        Position PlayerSpawnPosition { get; }

        IEnumerable<GroundTile> GetGroundTiles();
        IEnumerable<MapObject> GetMapObjects();
    }
    
    internal class GameMap : IGameMap
    {
        public string Name { get; set; }

        public int Width => _groundLayer.GetLength(0);
        public int Height => _groundLayer.GetLength(1);

        private int[,] _groundLayer { get; set; }
        private int[,] _objectLayer { get; set; }

        public GroundTile[,] GroundLayer { get; }
        public MapObject[,] ObjectsLayer { get; }
        public Point PlayerSpawnInTiles { get; }
        public Position PlayerSpawnPosition { get; }


        public GameMap(string name, int[,] groundLayer, int[,] objectLayer, Point playerSpawnInTiles)
        {
            Name = name;
            if (groundLayer.GetLength(0) != objectLayer.GetLength(0) || groundLayer.GetLength(1) != objectLayer.GetLength(1))
                throw new ArgumentException("Ground and object layers must have the same size");

            _groundLayer = groundLayer;
            _objectLayer = objectLayer;
            PlayerSpawnInTiles = CalculateTilePositionWithWaterInTiles(playerSpawnInTiles);
            PlayerSpawnPosition = ConvertTilesToPixels(PlayerSpawnInTiles);

            GroundLayer = new GroundTile[
                CalculateLengthWithWater(Width),
                CalculateLengthWithWater(Height)];
            ObjectsLayer = new MapObject[
                CalculateLengthWithWater(Width),
                CalculateLengthWithWater(Height)];
            InitMap();
        }

        private Position ConvertTilesToPixels(Point tile)
        {
            return new Position(tile.X * Constants.TileWidth, tile.Y * Constants.TileHeight);
        }

        private void InitMap()
        {
            var groundLayerWithWater = ExpandLayerWithWaterBorders(_groundLayer);
            for (int row = 0; row < groundLayerWithWater.GetLength(0); row++)
            {
                for (int column = 0; column < groundLayerWithWater.GetLength(1); column++)
                {
                    GroundLayer[row, column] = new GroundTile(
                        ConvertTilesToPixels(new Point(column, row)),
                        (GroundTileType)groundLayerWithWater[row, column]);
                }
            }

            var objectLayerWithWater = ExpandLayerWithWaterBorders(_objectLayer);
            for (int row = 0; row < objectLayerWithWater.GetLength(0); row++)
            {
                for (int column = 0; column < objectLayerWithWater.GetLength(1); column++)
                {
                    ObjectsLayer[row, column] = new MapObject(
                        ConvertTilesToPixels(new Point(column, row)),
                        (MapObjectType)objectLayerWithWater[row, column]);
                }
            }
        }

        private int[,] ExpandLayerWithWaterBorders(int[,] layer)
        {
            var layerWithWater = new int[
                CalculateLengthWithWater(Width),
                CalculateLengthWithWater(Height)];

            for (int row = 0; row < layerWithWater.GetLength(0); row++)
            {
                for (int column = 0; column < layerWithWater.GetLength(1); column++)
                {
                    layerWithWater[row, column] = (int)GroundTileType.Water;
                }
            }

            for (int row = 0; row < layer.GetLength(0); row++)
            {
                for (int column = 0; column < layer.GetLength(1); column++)
                {
                    var originalPosition = CalculateTilePositionWithWaterInTiles(new Point(row, column));
                    layerWithWater[originalPosition.X, originalPosition.Y] = layer[row, column];
                }
            }

            return layerWithWater;
        }

        private int CalculateLengthWithWater(int mapLength) => mapLength + 2 * Constants.WaterBorderSizeInTiles;

        private Point CalculateTilePositionWithWaterInTiles(Point point) =>
            new Point(point.X + Constants.WaterBorderSizeInTiles, point.Y + Constants.WaterBorderSizeInTiles);

        public IEnumerable<GroundTile> GetGroundTiles()
        {
            for (int row = 0; row < GroundLayer.GetLength(0); row++)
            {
                for (int column = 0; column < GroundLayer.GetLength(1); column++)
                {
                    yield return GroundLayer[row, column];
                }
            }
        }
        
        public IEnumerable<MapObject> GetMapObjects()
        {
            for (int row = 0; row < ObjectsLayer.GetLength(0); row++)
            {
                for (int column = 0; column < ObjectsLayer.GetLength(1); column++)
                {
                    if (ObjectsLayer[row, column].Type != MapObjectType.None)
                    {
                        yield return ObjectsLayer[row, column];
                    }
                }
            }
        }
    }
}
