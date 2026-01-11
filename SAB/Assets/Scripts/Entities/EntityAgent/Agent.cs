namespace SAB.EntityAgent
{
    /// <summary>
    /// 행동 전략과 객체를 입력받아 수행
    /// </summary>
    public class Agent
    {
        public readonly int ID;
        private int _receiverID;

        private IBrainStrategy _behaviorStrategy;
        private ICombatReceiver _combatReceiver;
        private IMovementReceiver _movementReceiver;

        private bool _isActivation;

        public int ReceiverID
        {
            get => _receiverID;
        }

        public bool IsActivation
        {
            get => _isActivation;
        }

        public Agent(int id)
        {
            ID = id;
        }

        public void Tick()
        {
            _behaviorStrategy.Tick();
        }

        public void SetActive(bool isActivation)
        {
            if (isActivation == _isActivation)
                return;

            _isActivation = isActivation;
            if (isActivation == true)
                _behaviorStrategy.Enable();
            else
                _behaviorStrategy.Disable();
        }

        public void SetBehaviorStrategy(IBrainStrategy unitBehavior)
        {
            _behaviorStrategy?.Disable();

            _behaviorStrategy = unitBehavior;
            RefreshBehaviorStrategy();
            if (_isActivation == true)
                _behaviorStrategy.Enable();
        }

        public void SetReceivers<T>(T receiver) where T : IInputReceiver
        {
            SetActionReceiver(receiver as ICombatReceiver);
            SetMovementReceiver(receiver as IMovementReceiver);
            _receiverID = receiver.ReceiverID;
        }

        private void SetActionReceiver(ICombatReceiver receiver)
        {
            _combatReceiver = receiver;
            _behaviorStrategy.SetCombatReceiver(_combatReceiver);
        }

        private void SetMovementReceiver(IMovementReceiver receiver)
        {
            _movementReceiver = receiver;
            _behaviorStrategy.SetMovementReceiver(_movementReceiver);
        }

        private void RefreshBehaviorStrategy()
        {
            _behaviorStrategy.SetCombatReceiver(_combatReceiver);
            _behaviorStrategy.SetMovementReceiver(_movementReceiver);
        }
    }

}