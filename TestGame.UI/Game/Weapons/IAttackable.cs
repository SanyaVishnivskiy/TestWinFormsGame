namespace TestGame.UI.Game.Weapons
{
    public interface IAttackable
    {
        AttackDetails? Attack();
        AttackDetails? AttackTick();
    }
}
