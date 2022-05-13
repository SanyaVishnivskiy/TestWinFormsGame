namespace TestGame.UI.Game.World.Tiles;

internal class WaterTile : GroundTile, ICollidable
{
    public WaterTile(Position position) : base(position)
    {
        Animation = Animation.New()
            .FromSprite(Textures.Water)
            .WithFirstFrame(new Rectangle(0, 0, 32, 32))
            .WithFrameCount(2)
            .WithFrameDelay(TimeSpan.FromMilliseconds(800))
            .Looped()
            .Build();

        Hitbox = new RectangleF(Position.X, Position.Y, Constants.TileWidth, Constants.TileHeight);
    }


    public override TileType Type => TileType.Water;

    public override Animation Animation { get; }

    public RectangleF Hitbox { get; }
}
