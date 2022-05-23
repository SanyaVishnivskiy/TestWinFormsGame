namespace TestGame.UI.Game.Weapons.Melee
{
    public class Dagger : Weapon
    {
        public Dagger()
        {
            Width = 8;
            Height = 24;
            Animation = WeaponAnimations.MeleeDefaultAnimation;
            AttackBehavior = new DaggerAttackBehaviour(this);
            AttackCoolDown = TimeSpan.FromMilliseconds(200);
            AttackDuration = TimeSpan.FromSeconds(1);
            Damage = 1;
        }

        public override WeaponType Type => WeaponType.Melee;
    }
}
