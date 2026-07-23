namespace SAB.Unit
{
    public class StateContext
    {
        private bool _isMoveing;
        private bool _isAttacking;

        private bool _isInCombat;
        private float _remainBattleTime;

        public bool IsMoveing
        {
            get => _isMoveing;
            set => _isMoveing = value;
        }

        public bool IsCombatMode
        {
            get => _isInCombat;
            set => _isInCombat = value;
        }

        public float RemainBattileTime
        {
            get => _remainBattleTime;
            set => _remainBattleTime = value;
        }

        public bool IsAttacking
        {
            get => _isAttacking;
            set => _isAttacking = value;
        }
    }
}
