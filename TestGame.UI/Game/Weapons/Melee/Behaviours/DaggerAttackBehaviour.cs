namespace TestGame.UI.Game.Weapons.Melee
{
    public class DaggerAttackBehaviour : IAttackBehavior
    {
        public const int HandLeftXOffset = 5;
        public const int HandLeftYOffsetDivider = 4;
        public const int HandRightXOffset = 3;
        public const int HandRightYOffsetDivider = 2;
        public const int HandUpYOffsetDivider = 2;
        public const int HandDownXOffsetDivider = 2;
        public const int HandDownYOffsetDivider = 2;

        public const int HandLeftXMaxLength = 4;
        public const int HandRightXMaxLength = 20;
        public const int HandUpYMaxLength = 22;
        public const int HandDownYMaxLength = 9;

        public const int HandSize = 4;

        public const int HandleOffset = 2;

        public Guid AttackId { get; protected set; }
        public RectangleF? Hitbox { get; private set; }
        public DateTime? AttackStartedAt { get; protected set; }
        public Direction AttackDirection { get; protected set; }
        public DateTime? AttackFinishedAt { get; protected set; }
        public TimeSpan AttackDuration { get; protected set; }
        public TimeSpan AttackCoolDown { get; protected set; }
        public bool Attacking => Hitbox is not null;
        public bool AttackExpired => DateTime.Now - (AttackStartedAt ?? DateTime.MinValue) > AttackDuration;
        
        private readonly Weapon _weapon;

        public DaggerAttackBehaviour(Weapon weapon)
        {
            _weapon = weapon;
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

        private RectangleF GetInitialHitboxPosition(Entity entity)
        {
            var weaponOffset = GetInitialWeaponOffset(entity);
            var weaponRect = GetWeaponRectangle(AttackDirection);
            return new RectangleF(weaponOffset.X, weaponOffset.Y, weaponRect.Width, weaponRect.Height);
        }

        //TODO: refactor this
        private PointF GetInitialWeaponOffset(Entity entity)
        {
            var halfWidth = entity.Width / 2;
            var halfHeight = entity.Height / 2;
            var weaponRectangle = GetWeaponRectangle(AttackDirection);
            if (AttackDirection.Horizontal == MoveDirection.Left)
            {
                var x = entity.Position.X + HandLeftXOffset + HandleOffset - weaponRectangle.Width;
                var y = entity.Position.Y + halfHeight + (halfHeight / HandLeftYOffsetDivider) + HandleOffset
                    - (weaponRectangle.Height / 2);
                return new PointF(x, y);
            }
            else if (AttackDirection.Horizontal == MoveDirection.Right)
            {
                var x = entity.Position.X + halfWidth - HandRightXOffset - HandleOffset;
                var y = entity.Position.Y + halfHeight + (halfHeight / HandRightYOffsetDivider) - (weaponRectangle.Height / 2);
                return new PointF(x, y);
            }
            else if (AttackDirection.Vertical == MoveDirection.Up)
            {
                var x = entity.Position.X + entity.Width - HandSize - 1 - (weaponRectangle.Width / 2);
                var y = entity.Position.Y + halfHeight + (halfHeight / HandUpYOffsetDivider) + 1 - weaponRectangle.Height + HandleOffset;
                return new PointF(x, y);
            }
            else
            {
                var x = entity.Position.X + halfWidth - (halfWidth / HandDownXOffsetDivider) - 1 - (weaponRectangle.Width / 2);
                var y = entity.Position.Y + halfHeight + (halfHeight / HandDownYOffsetDivider) - 2;
                return new PointF(x, y);
            }
        }

        private RectangleF GetWeaponRectangle(Direction faceDirection)
        {
            if (faceDirection.IsHorizontal)
            {
                return new RectangleF(0, 0, _weapon.Height, _weapon.Width);
            }

            return new RectangleF(0, 0, _weapon.Width, _weapon.Height);
        }

        private void UpdatePosition(Entity entity)
        {
            var hitbox = GetInitialHitboxPosition(entity);
            hitbox = MoveHitboxDependingOnAttackTime(hitbox);
            Hitbox = hitbox;
        }

        private RectangleF MoveHitboxDependingOnAttackTime(RectangleF hitbox)
        {
            var attackTime = DateTime.Now - (AttackStartedAt ?? DateTime.MinValue);
            var halfDuration = AttackDuration / 2;
            var isFirstHalf = attackTime <= halfDuration;

            float positionDelta = isFirstHalf
                ? (float)attackTime.Ticks / halfDuration.Ticks
                : 1 - (((float)attackTime.Ticks - halfDuration.Ticks) / halfDuration.Ticks);
            Logger.Log(LogCategory.Attack, $"Position delta: {positionDelta}, attackTime: {attackTime.Ticks}, halfDuration: {halfDuration.Ticks}");

            var positionOffset = GetWeaponOffsetFromInitial(hitbox, AttackDirection, positionDelta);
            return new RectangleF(positionOffset, hitbox.Size);
        }

        private PointF GetWeaponOffsetFromInitial(RectangleF hitbox, Direction faceDirection, float positionDelta)
        {
            if (faceDirection.Horizontal == MoveDirection.Left)
            {
                return new PointF(hitbox.X - (int)(HandLeftXMaxLength * positionDelta), hitbox.Y);
            }
            else if (faceDirection.Horizontal == MoveDirection.Right)
            {
                return new PointF(hitbox.X + (int)(HandRightXMaxLength * positionDelta), hitbox.Y);
            }
            else if (faceDirection.Vertical == MoveDirection.Up)
            {
                return new PointF(hitbox.X, hitbox.Y - (int)(HandUpYMaxLength * positionDelta));
            }
            else
            {
                return new PointF(hitbox.X, hitbox.Y + (int)(HandDownYMaxLength * positionDelta));
            }
        }
    }
}
