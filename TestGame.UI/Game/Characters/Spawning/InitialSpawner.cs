namespace TestGame.UI.Game.Characters.Spawning
{
    public class InitialSpawner : Spawner
    {
        private readonly Func<Position, Entity> _spawnFunc;

        public InitialSpawner(Rectangle spawnRadiusInTiles, Func<Position, Entity> spawnFunc) 
            : base(spawnRadiusInTiles, 1, default)
        {
            _spawnFunc = spawnFunc;
        }

        public override SpawningType Type => SpawningType.Initial;

        protected override Entity GetNextEntityToSpawn(Position position)
        {
            return _spawnFunc(position);
        }
    }
}
