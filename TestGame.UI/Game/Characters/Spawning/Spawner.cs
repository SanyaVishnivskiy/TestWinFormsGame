namespace TestGame.UI.Game.Characters.Spawning;

public enum SpawningType
{
    Initial,
    Sequential,
}

public abstract class Spawner
{
    private readonly GameState _state;
    private readonly TimeSpan _spawnFrequency;
    private readonly int _spawnTimes;
    private readonly Rectangle _spawnRadiusInTiles;

    private int _entitiesSpawned;
    private DateTime _previousSpawnedAt;

    public abstract SpawningType Type { get; }

    public Spawner(Rectangle spawnRadiusInTiles, int spawnTimes = 1, TimeSpan spawnFrequency = default)
    {
        _state = GameState.Instance;
        _spawnFrequency = spawnFrequency;
        _spawnTimes = spawnTimes;
        _spawnRadiusInTiles = spawnRadiusInTiles;
    }

    public void Spawn()
    {
        if (!SpawnNeeded())
        {
            return;
        }

        var position = GetNextEntityPosition();
        if (position is null)
        {
            return;
        }

        var entity = GetNextEntityToSpawn(position);
        entity.EnsureInitialized();

        Spawn(entity);

        _entitiesSpawned++;
        _previousSpawnedAt = DateTime.Now;
    }

    private bool SpawnNeeded()
    {
        if (_entitiesSpawned > _spawnTimes)
        {
            return false;
        }

        if (_previousSpawnedAt.Add(_spawnFrequency) > DateTime.Now)
        {
            return false;
        }

        return true;
    }

    private Position? GetNextEntityPosition()
    {
        if (_spawnRadiusInTiles.Size.IsEmpty)
        {
            return ConvertToPositionIfValid(_spawnRadiusInTiles.X, _spawnRadiusInTiles.Y);
        }

        var possibleSpawns = Enumerable.Range(0, _spawnRadiusInTiles.Width * _spawnRadiusInTiles.Height).ToList();

        while (possibleSpawns.Count > 0)
        {
            var randomIndex = Random.Shared.Next(possibleSpawns.Count);
            var randomTileNumber = possibleSpawns[randomIndex];

            var y = (int)Math.Floor((double)randomTileNumber / _spawnRadiusInTiles.Width);
            var x = randomTileNumber % _spawnRadiusInTiles.Width;
            var position = ConvertToPositionIfValid(x, y);
            if (position is not null)
            {
                return position;
            }
        }

        return null;
    }

    private Position? ConvertToPositionIfValid(int x, int y)
    {
        var spawnTile = _state.Map.CalculateTilePositionWithWaterInTiles(new Point(x, y));
        var validSpawn = _state.Map.CheckValidSpawnWithWater(spawnTile);
        if (!validSpawn)
        {
            return null;
        }

        return TileToPositionConverter.ConvertTilesToPosition(spawnTile);
    }

    protected abstract Entity GetNextEntityToSpawn(Position position);

    private void Spawn(Entity entity)
    {
        BeforeSpawn(entity);
        _state.AddGameEntity(entity);
    }

    protected virtual void BeforeSpawn(Entity entity)
    {
    }
}
