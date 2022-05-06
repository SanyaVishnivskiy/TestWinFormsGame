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
            PlayerSpawnInTiles = playerSpawnInTiles;
            PlayerSpawnPosition = ConvertTilesToPixels(playerSpawnInTiles);

            GroundLayer = new GroundTile[Width, Height];
            ObjectsLayer = new MapObject[Width, Height];
            InitMap();
        }

        private Position ConvertTilesToPixels(Point tile)
        {
            return new Position(tile.X * Constants.TileWidth, tile.Y * Constants.TileHeight);
        }

        private void InitMap()
        {
            for (int row = 0; row < _groundLayer.GetLength(0); row++)
            {
                for (int column = 0; column < _groundLayer.GetLength(1); column++)
                {
                    GroundLayer[row, column] = new GroundTile(
                        ConvertTilesToPixels(new Point(column, row)),
                        (GroundTileType)_groundLayer[row, column]);
                }
            }

            for (int row = 0; row < _objectLayer.GetLength(0); row++)
            {
                for (int column = 0; column < _objectLayer.GetLength(1); column++)
                {
                    ObjectsLayer[row, column] = new MapObject(
                        ConvertTilesToPixels(new Point(column, row)),
                        (MapObjectType)_objectLayer[row, column]);
                }
            }
        }

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
            for (int row = 0; row < GroundLayer.GetLength(0); row++)
            {
                for (int column = 0; column < GroundLayer.GetLength(1); column++)
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
