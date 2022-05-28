namespace TestGame.UI.Game.Characters.Enemies
{
    public class Dummy : Entity, ICollidable
    {
        public Dummy(Position position) : base(position, EntitiesAnimations.DummyAnimation)
        {
            Hitbox = new RectangleF(Position.X, Position.Y, Width, Height);
            Moving = MovingInfo.None;
            MovableBehaviour = new NoMovingBehaviour(position);
            Health = new Health(10, new Size(Width, Height));
        }

        public RectangleF Hitbox { get; }
    }
}
