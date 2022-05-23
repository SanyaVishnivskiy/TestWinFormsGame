namespace TestGame.UI.Game.Weapons
{
    public interface IAttackBehavior
    {
        bool Attacking { get; }
        RectangleF? Hitbox { get; }
        bool AttackExpired { get; }
        AttackDetails TryBeginAttack(Entity owner);
        AttackDetails AttackTick(Entity owner);
    }

    public class AttackDetails
    {
        public bool AttackStarted { get; }
        public bool AttackFinished { get; }
        public bool AttackInProgress { get; }

        private AttackDetails(bool attackStarted, bool attackFinished, bool attacking)
        {
            AttackStarted = attackStarted;
            AttackFinished = attackFinished;
            AttackInProgress = attacking;
        }

        public static AttackDetails Started()
        {
            return new AttackDetails(true, false, false);
        }

        public static AttackDetails Finished()
        {
            return new AttackDetails(false, true, false);
        }

        public static AttackDetails None()
        {
            return new AttackDetails(false, false, false);
        }

        public static AttackDetails Attacking()
        {
            return new AttackDetails(false, false, true);
        }
    }
}