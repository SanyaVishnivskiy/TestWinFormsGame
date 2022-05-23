namespace TestGame.UI.Game.Characters.Spawning
{
    public class PlayerSpawner : InitialSpawner
    {
        public PlayerSpawner(Point playerSpawnerInTiles)
            : base(new Rectangle(playerSpawnerInTiles, new Size(0, 0)), p => new Player(p))
        {
        }

        protected override void BeforeSpawn(Entity entity)
        {
            GameState.Instance.Player = (entity as Player)!;
        }
    }
}
