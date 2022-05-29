namespace TestGame.UI.Game.Weapons
{
    public interface IAttackBehavior
    {
        bool Attacking { get; }
        RectangleF? Hitbox { get; }
        Direction AttackDirection { get; }
        bool AttackExpired { get; }
        AttackDetails TryBeginAttack(Entity owner);
        AttackDetails AttackTick(Entity owner);
    }

    public class AttackDetails
    {
        public Guid AttackId { get; }
        public bool AttackStarted { get; }
        public bool AttackFinished { get; }
        public bool AttackInProgress { get; }

        private AttackDetails(Guid id, bool attackStarted, bool attackFinished, bool attacking)
        {
            AttackId = id;
            AttackStarted = attackStarted;
            AttackFinished = attackFinished;
            AttackInProgress = attacking;
        }

        public static AttackDetails Started(Guid id)
        {
            return new AttackDetails(id, true, false, false);
        }

        public static AttackDetails Finished(Guid id)
        {
            return new AttackDetails(id, false, true, false);
        }

        public static AttackDetails None()
        {
            return new AttackDetails(Guid.Empty, false, false, false);
        }

        public static AttackDetails Attacking(Guid id)
        {
            return new AttackDetails(id, false, false, true);
        }
    }
}