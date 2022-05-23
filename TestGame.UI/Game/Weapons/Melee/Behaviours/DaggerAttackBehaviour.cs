﻿namespace TestGame.UI.Game.Weapons.Melee
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

        public RectangleF? Hitbox { get; private set; }
        public DateTime? AttackStartedAt { get; protected set; }
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
                return AttackDetails.Attacking();
            }

            if (AttackFinishedAt.HasValue && DateTime.Now - AttackFinishedAt.Value < AttackCoolDown)
            {
                return AttackDetails.None();
            }

            var position = GetInitialHitboxPosition(owner);
            Hitbox = position;
            AttackStartedAt = DateTime.Now;
            return AttackDetails.Started();
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
                return AttackDetails.Finished();
            }

            UpdatePosition(owner);
            return AttackDetails.Attacking();
        }

        private RectangleF GetInitialHitboxPosition(Entity entity)
        {
            var weaponOffset = GetInitialWeaponOffset(entity);
            return new RectangleF(weaponOffset.X, weaponOffset.Y, _weapon.Width, _weapon.Height);
        }

        //TODO: refactor this
        private PointF GetInitialWeaponOffset(Entity entity)
        {
            var halfWidth = entity.Width / 2;
            var halfHeight = entity.Height / 2;
            var weaponRectangle = GetWeaponRectangle(entity.FaceDirection);
            if (entity.FaceDirection.Horizontal == MoveDirection.Left)
            {
                var x = entity.Position.X + HandLeftXOffset + HandleOffset - weaponRectangle.Width;
                var y = entity.Position.Y + halfHeight + (halfHeight / HandLeftYOffsetDivider) + HandleOffset
                    - (weaponRectangle.Height / 2);
                return new PointF(x, y);
            }
            else if (entity.FaceDirection.Horizontal == MoveDirection.Right)
            {
                var x = entity.Position.X + halfWidth - HandRightXOffset - HandleOffset;
                var y = entity.Position.Y + halfHeight + (halfHeight / HandRightYOffsetDivider) - (weaponRectangle.Height / 2);
                return new PointF(x, y);
            }
            else if (entity.FaceDirection.Vertical == MoveDirection.Up)
            {
                var x = entity.Position.X + entity.Width - HandSize - 1 - (weaponRectangle.Width / 2);
                var y = entity.Position.Y + halfHeight + (halfHeight / HandUpYOffsetDivider) + 1 - weaponRectangle.Height + HandleOffset;
                return new PointF(x, y);
            }
            else
            {
                var x = entity.Position.X + halfWidth + (halfWidth / HandDownXOffsetDivider) - 1 - (weaponRectangle.Width / 2);
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
            hitbox = MoveHitboxDependingOnAttackTime(entity, hitbox);
            Hitbox = hitbox;
        }

        private RectangleF MoveHitboxDependingOnAttackTime(Entity entity, RectangleF hitbox)
        {
            var attackTime = DateTime.Now - (AttackStartedAt ?? DateTime.MinValue);
            var halfDuration = AttackDuration / 2;
            var isFirstHalf = attackTime <= halfDuration;
            float positionDelta = isFirstHalf
                ? attackTime.Ticks / halfDuration.Ticks
                : 1 - (attackTime.Ticks / halfDuration.Ticks);
            var positionOffset = GetWeaponOffsetFromInitial(hitbox, entity.FaceDirection, positionDelta);
            return new RectangleF(positionOffset, hitbox.Size);
        }

        private PointF GetWeaponOffsetFromInitial(RectangleF hitbox, Direction faceDirection, float positionDelta)
        {
            if (faceDirection.Horizontal == MoveDirection.Left)
            {
                return new PointF(hitbox.X - HandLeftXMaxLength * positionDelta, hitbox.Y);
            }
            else if (faceDirection.Horizontal == MoveDirection.Right)
            {
                return new PointF(hitbox.X + HandRightXMaxLength * positionDelta, hitbox.Y);
            }
            else if (faceDirection.Vertical == MoveDirection.Up)
            {
                return new PointF(hitbox.X, hitbox.Y - HandUpYMaxLength * positionDelta);
            }
            else
            {
                return new PointF(hitbox.X, hitbox.Y + HandDownYMaxLength * positionDelta);
            }
        }
    }
}