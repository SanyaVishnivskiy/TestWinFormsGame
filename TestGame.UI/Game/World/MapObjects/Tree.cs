namespace TestGame.UI.Game.World.MapObjects
{
    public class Tree : MapObject, ICollidable
    {
        public override MapObjectType Type => MapObjectType.Tree;

        public override Animation Animation { get; }
        public override int Width { get; }
        public override int Height { get; }

        public RectangleF Hitbox { get; }

        public Tree(Position position) : base(position)
        {
            Animation = Animation.New()
                .FromSprite(Textures.Tree)
                .WithFirstFrame(new Rectangle(0, 0, 32, 32 * 2))
                .Build();

            Width = Constants.TileWidth * 2;
            Height = Constants.TileHeight * 2 * 2;
            Hitbox = new RectangleF(Position.X, Position.Y + 3 * Height / 4, Width, Height / 4);
        }
    }
}
