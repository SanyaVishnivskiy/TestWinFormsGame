namespace TestGame.UI.Game.Weapons.Melee
{
    public class DaggerAttackBehaviour : AttackBehaviourBase
    {
        public const int HandLeftXOffset = 5;
        public const int HandLeftYOffsetDivider = 4;
        public const int HandRightXOffset = 3;
        public const int HandRightYOffsetDivider = 2;
        public const int HandUpYOffsetDivider = 2;
        public const int HandDownXOffsetDivider = 2;
        public const int HandDownYOffsetDivider = 2;

        public const int HandLeftXMaxLength = 5;
        public const int HandRightXMaxLength = 20;
        public const int HandUpYMaxLength = 22;
        public const int HandDownYMaxLength = 9;

        public const int HandSize = 4;

        public const int HandleOffset = 2;

        public DaggerAttackBehaviour(Weapon weapon) : base(weapon)
        {
        }

        protected override RectangleF GetInitialHitboxPosition(Entity entity)
        {
            var weaponOffset = GetInitialWeaponOffset(entity);
            var weaponPosition = new PointF(entity.Position.X + weaponOffset.X, entity.Position.Y + weaponOffset.Y);
            var weaponRect = GetWeaponRectangle(AttackDirection);
            return new RectangleF(weaponPosition, weaponRect.Size);
        }

        //TODO: refactor this
        private PointF GetInitialWeaponOffset(Entity entity)
        {
            var halfWidth = entity.Width / 2;
            var halfHeight = entity.Height / 2;
            var weaponRectangle = GetWeaponRectangle(AttackDirection);
            if (AttackDirection.Horizontal == MoveDirection.Left)
            {
                var x = HandLeftXOffset + HandleOffset - weaponRectangle.Width;
                var y = halfHeight + (halfHeight / HandLeftYOffsetDivider) + HandleOffset - (weaponRectangle.Height / 2);
                return new PointF(x, y);
            }
            else if (AttackDirection.Horizontal == MoveDirection.Right)
            {
                var x = halfWidth - HandRightXOffset - HandleOffset;
                var y = halfHeight + (halfHeight / HandRightYOffsetDivider) - (weaponRectangle.Height / 2);
                return new PointF(x, y);
            }
            else if (AttackDirection.Vertical == MoveDirection.Up)
            {
                var x = entity.Width - HandSize - 1 - (weaponRectangle.Width / 2);
                var y = halfHeight + (halfHeight / HandUpYOffsetDivider) + 1 - weaponRectangle.Height + HandleOffset;
                return new PointF(x, y);
            }
            else
            {
                var x = halfWidth - (halfWidth / HandDownXOffsetDivider) - 1 - (weaponRectangle.Width / 2);
                var y = halfHeight + (halfHeight / HandDownYOffsetDivider) - 2;
                return new PointF(x, y);
            }
        }

        private RectangleF GetWeaponRectangle(Direction faceDirection)
        {
            if (faceDirection.IsHorizontal)
            {
                return new RectangleF(0, 0, Weapon.Height, Weapon.Width);
            }

            return new RectangleF(0, 0, Weapon.Width, Weapon.Height);
        }

        protected override RectangleF GetWeaponHitbox(Entity entity)
        {
            var hitbox = GetInitialHitboxPosition(entity);
            return MoveHitboxDependingOnAttackTime(hitbox);
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
