namespace TestGame.UI.Game.World.Tiles;

public class GrassTile : GroundTile
{
    public GrassTile(Position position) : base(position)
    {
        Animation = Animation.New()
            .FromSprite(Textures.Grass)
            .Build();
    }


    public override TileType Type => TileType.Grass;

    public override Animation Animation { get; }
}
