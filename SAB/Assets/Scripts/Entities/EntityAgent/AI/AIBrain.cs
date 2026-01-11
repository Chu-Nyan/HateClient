using ChampagneSupernova.Library.BehaviorTree;
using SAB.EntityAgent.AI.Context;
using SAB.EntityAgent.AI.StateMachine;
using System.Collections.Generic;

namespace SAB.EntityAgent.AI
{
    /// <summary>
    /// AI를 최종적으로 실행하는 최상위 컨트롤러
    /// </summary>
    public class AIBrain : IBrainStrategy
    {
        private readonly Dictionary<CommandType, IMachineState<AIContext>> _stateByOrder;
        private BehaviorAI<AIContext> _commandAI;
        private StateMachine.StateMachine _stateMachine;
        private AIContext _context;

        public AIBrain(BehaviorAI<AIContext> brain)
        {
            _commandAI = brain;

            _stateMachine = new();
            _context = new();
            _stateByOrder = new();

            _stateByOrder[CommandType.Idle] = new IdleState();
            _stateByOrder[CommandType.Move] = new MoveState();
        }
        public void Tick()
        {
            if (_stateMachine.IsDone == true)
            {
                _commandAI.Execute(_context);
                var nextState = _stateByOrder[_context.Command];
                _stateMachine.SetAIState(nextState, _context);
            }

            _stateMachine.Tick(_context);
        }

        public void Enable()
        {
            _commandAI.SetActive(true);
        }

        public void Disable()
        {
            _commandAI.SetActive(false);
        }

        public void SetCombatReceiver(IOffenseReceiver receiver)
        {
            _context.CombatReceiver = receiver;
        }

        public void SetMovementReceiver(IMovementReceiver receiver)
        {
            _context.MovementReceiver = receiver;
        }
    }
}
