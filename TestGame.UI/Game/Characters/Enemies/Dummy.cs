namespace TestGame.UI.Game.Characters.Enemies
{
    public class Dummy : Entity, ICollidable
    {
        public Dummy(Position position) : base(position, EntitiesAnimations.DummyAnimation)
        {
            Hitbox = new RectangleF(Position.X, Position.Y, Width, Height);
        }

        public RectangleF Hitbox { get; }
    }
}
