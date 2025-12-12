using ChampagneSupernova.Library.BehaviorTree;

namespace SAB.AI.Data
{
    /// <summary>
    /// 상황 판단, 명령 전달에 필요한 데이터
    /// </summary>
    public class AIContext
    {
        private IMovementReceiver _movementReceiver;
        private ICombatReceiver _combatReceiver;

        private CommandType _command;
        private MethodResult _progress;

        private MoveCommandData _moveCommandData;
        private IdleCommandData _idleCommandData;

        public IMovementReceiver MovementReceiver
        {
            get => _movementReceiver;
            set => _movementReceiver = value;
        }

        public ICombatReceiver CombatReceiver
        { 
            get => _combatReceiver;
            set => _combatReceiver = value;
        }

        public CommandType Command 
        { 
            get => _command;
        }

        public MethodResult Progress
        {
            get => _progress;
            set => _progress = value;
        }
        public MoveCommandData MoveCommandData 
        { 
            get => _moveCommandData;
        }

        public IdleCommandData IdleCommandData
        {
            get => _idleCommandData;
        }

        public void OrderToMove(MoveCommandData data)
        {
            _command = CommandType.Move;
            Progress = MethodResult.Running;

            _moveCommandData = data;
        }

        public void OrderToPatrol(MoveCommandData data)
        {
            _command = CommandType.Patrol;
            Progress = MethodResult.Running;

            _moveCommandData = data;
        }

        public void OrderToIdle(IdleCommandData data)
        {
            _command = CommandType.Idle;
            Progress = MethodResult.Running;

            _idleCommandData = data;
        }
    }
}
