namespace TestGame.UI.Game.Weapons
{
    public abstract class Weapon : IRenderable
    {
        public Weapon()
        {
            Damage = 1;
            AttackDuration = TimeSpan.FromSeconds(1);
            AttackCoolDown = TimeSpan.FromMilliseconds(200);
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int CurrentTextureWidth => (int)(AttackBehavior.Hitbox?.Width ?? 0);
        public int CurrentTextureHeight => (int)(AttackBehavior.Hitbox?.Height ?? 0);

        public abstract WeaponType Type { get; }

        public AnimationAggregate Animation { get; protected set; }
        public IAttackBehavior AttackBehavior { get; protected set; }

        public float Damage { get; protected set; }
        public TimeSpan AttackDuration { get; protected set; }
        public TimeSpan AttackCoolDown { get; protected set; }

        public bool AttackExpired => AttackBehavior.AttackExpired;

        public bool Attacking => AttackBehavior.Attacking;

        public Position Position
        {
            get
            {
                if (AttackBehavior.Hitbox is null)
                {
                    return null;
                }

                return new Position(AttackBehavior.Hitbox.Value.X, AttackBehavior.Hitbox.Value.Y);
            }
        }

        public AttackDetails TryBeginAttack(Entity owner)
        {
            return AttackBehavior.TryBeginAttack(owner);
        }

        public AttackDetails AttackTick(Entity owner)
        {
            return AttackBehavior.AttackTick(owner);
        }

        public Bitmap? GetTexture()
        {
            if (AttackBehavior.Hitbox is null)
            {
                return null;
            }

            return Animation.GetNextFrame();
        }
    }

    public enum WeaponType
    {
        Melee,
    }
}
