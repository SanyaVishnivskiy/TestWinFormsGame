namespace TestGame.UI.Game.Weapons.Melee
{
    public class LightenedDagger : Weapon
    {
        public override WeaponType Type => WeaponType.Melee;

        public LightenedDagger()
        {
            Damage = 1;
            AttackDuration = TimeSpan.FromMilliseconds(300);
            AttackCoolDown = TimeSpan.FromMilliseconds(50);
            Width = 8;
            Height = 24;
            Animation = WeaponAnimations.MeleeDefaultAnimation;
            AttackBehavior = new DaggerAttackBehaviour(this);
        }
    }
}
