using SAB.GameSystem;

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

        private bool _isActivation;

        public int ReceiverID
        {
            get => _receiverID;
        }

        public bool IsActivation
        {
            get => _isActivation;
        }

        public Agent(int id, IBrainStrategy unitBehavior)
        {
            ID = id;
            _behaviorStrategy = unitBehavior;
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

        public void SetReceivers<T>(T receiver) where T : IInputReceiver
        {
            if (receiver is IOffenseReceiver offense)
                SetActionReceiver(offense);
            if (receiver is IMovementReceiver movement)
                SetMovementReceiver(movement);

            _receiverID = receiver.ReceiverID;
            receiver.OnOwnerChanged(_behaviorStrategy.BrainType);
        }

        private void SetActionReceiver(IOffenseReceiver receiver)
        {
            _behaviorStrategy.SetCombatReceiver(receiver);
        }

        private void SetMovementReceiver(IMovementReceiver receiver)
        {
            _behaviorStrategy.SetMovementReceiver(receiver);
        }
    }
}