namespace TestGame.UI.Game.Characters.Enemies
{
    public class Enemy : Entity
    {
        public Enemy(Position position, AnimationAggregate animation) : base(position, animation)
        {
        }

        public override bool IsEnemy(Entity e)
        {
            return e is Player;
        }
    }
}
