namespace TestGame.UI.Game.World.Tiles;

public class DirtTile : GroundTile
{
    public DirtTile(Position position) : base(position)
    {
        Animation = Animation.New()
            .FromSprite(Textures.Dirt)
            .Build();
    }


    public override TileType Type => TileType.Dirt;

    public override Animation Animation { get; }
}
