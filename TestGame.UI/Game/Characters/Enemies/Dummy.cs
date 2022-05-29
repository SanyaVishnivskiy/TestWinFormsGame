namespace TestGame.UI.Game.Characters.Enemies
{
    public class Dummy : Enemy, ICollidable
    {
        public Dummy(Position position) : base(position, EntitiesAnimations.DummyAnimation)
        {
            Moving = MovingInfo.None;
            MovableBehaviour = new NoMovingBehaviour(position);
            Health = new Health(100, new Size(Width, Height));
        }
    }
}
