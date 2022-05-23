namespace TestGame.UI.Game.Weapons
{
    public class AttackingEngine : IDisposable
    {
        private readonly GameState _state;

        private IReadOnlyList<Entity> _attackingEntities;

        public AttackingEngine(GameState state)
        {
            _state = state;
            _state.OnMarkingAttack += OnWeaponActivation;
            OnWeaponActivation(this, new WeaponActivationEventArgs(_state.AttackingEntities));
        }

        private void OnWeaponActivation(object? sender, WeaponActivationEventArgs e)
        {
            _attackingEntities = e.Weapons;
        }

        public void ProcessAttacks()
        {
            UpdateAttacksTick();
        }

        private void UpdateAttacksTick()
        {
            foreach (var entity in _attackingEntities.ToList())
            {
                entity.AttackTick();
            }
        }

        public void Dispose()
        {
            _state.OnMarkingAttack -= OnWeaponActivation;
        }

        ~AttackingEngine()
        {
            Dispose();
        }
    }
}
