namespace TestGame.UI.Game.World
{
    public abstract class GroundTile : Tile
    {
        public GroundTile(Position position) : base(position)
        {
        }
    }

    public enum TileType
    {
        Water = 0,
        Dirt = 1,
        Grass = 2,
    }
}
