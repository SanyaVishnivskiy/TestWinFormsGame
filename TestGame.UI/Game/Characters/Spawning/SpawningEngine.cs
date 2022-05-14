namespace TestGame.UI.Game.Characters.Spawning;

public class SpawningEngine
{
    private readonly List<Spawner> _initialSpawners;
    private readonly List<Spawner> _spawners;

    public SpawningEngine()
    {
        var spawners = GetEntitiesSpawners();
        _initialSpawners = spawners.Where(x => x.Type == SpawningType.Initial).ToList();
        _spawners = spawners.Where(x => x.Type == SpawningType.Sequential).ToList();
    }

    private List<Spawner> GetEntitiesSpawners()
    {
        return new List<Spawner>
        {
            new SimpleSpawner(new Rectangle(2, 19, 0, 0), p => new Dummy(p))
        };
    }

    public void InitialSpawn()
    {
        foreach (var spawner in _initialSpawners)
        {
            spawner.Spawn();
        }
    }

    public void Spawn()
    {
        foreach (var spawner in _spawners)
        {
            spawner.Spawn();
        }
    }
}
