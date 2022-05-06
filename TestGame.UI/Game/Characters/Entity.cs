namespace TestGame.UI.Game.Characters
{
    internal class Entity : IRenderable
    {
        public Entity(Position position, Animation animation)
        {
            Position = position;
            Animation = animation;
        }

        public Position Position { get; }
        public Animation Animation { get; }

        public Bitmap GetFrame()
        {
            return Animation.GetNextFrame();
        }
    }
}
