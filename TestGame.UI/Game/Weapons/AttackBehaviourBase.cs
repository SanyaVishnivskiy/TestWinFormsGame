namespace TestGame.UI.Game.Weapons
{
    public abstract class AttackBehaviourBase : IAttackBehavior
    {
        public Guid AttackId { get; protected set; }
        public RectangleF? Hitbox { get; protected set; }
        public DateTime? AttackStartedAt { get; protected set; }
        public Direction AttackDirection { get; protected set; }
        public DateTime? AttackFinishedAt { get; protected set; }
        public TimeSpan AttackDuration { get; protected set; }
        public TimeSpan AttackCoolDown { get; protected set; }
        public bool Attacking => Hitbox is not null;
        public bool AttackExpired => DateTime.Now - (AttackStartedAt ?? DateTime.MinValue) > AttackDuration;

        protected Weapon Weapon { get; }

        public AttackBehaviourBase(Weapon weapon)
        {
            Weapon = weapon;
            AttackDuration = weapon.AttackDuration;
            AttackCoolDown = weapon.AttackCoolDown;
        }

        public AttackDetails TryBeginAttack(Entity owner)
        {
            if (Attacking)
            {
                return AttackDetails.Attacking(AttackId);
            }

            if (AttackFinishedAt.HasValue && DateTime.Now - AttackFinishedAt.Value < AttackCoolDown)
            {
                return AttackDetails.None();
            }

            AttackDirection = owner.FaceDirection;
            var position = GetInitialHitboxPosition(owner);
            Hitbox = position;
            AttackStartedAt = DateTime.Now;
            AttackId = Guid.NewGuid();
            return AttackDetails.Started(AttackId);
        }

        public AttackDetails AttackTick(Entity owner)
        {
            if (!Attacking)
            {
                return AttackDetails.None();
            }

            if (AttackExpired)
            {
                Hitbox = null;
                AttackStartedAt = null;
                var attackId = AttackId;
                AttackId = Guid.Empty;
                return AttackDetails.Finished(attackId);
            }

            UpdatePosition(owner);
            return AttackDetails.Attacking(AttackId);
        }

        private void UpdatePosition(Entity entity)
        {
            Hitbox = GetWeaponHitbox(entity);
        }
        protected abstract RectangleF GetInitialHitboxPosition(Entity entity);

        protected abstract RectangleF GetWeaponHitbox(Entity entity);
    }
}
