using ChampagneSupernova.Library.BehaviorTree;
using SAB.AI.Data;
using SAB.AI.Brain;
using System.Collections.Generic;

namespace SAB.AI
{
    /// <summary>
    /// AI를 최종적으로 실행하는 최상위 컨트롤러
    /// </summary>
    public class AIStrategy : IUnitBehaviorStrategy
    {
        private readonly Dictionary<CommandType, IMachineState<AIContext>> _stateByOrder;
        private BehaviorAI<AIContext> _commandAI;
        private StateMachine _stateMachine;
        private AIContext _context;

        public AIStrategy(BehaviorAI<AIContext> brain)
        {
            _commandAI = brain;

            _stateMachine = new();
            _context = new();
            _stateByOrder = new();

            _stateByOrder[CommandType.Idle] = new IdleState();
            _stateByOrder[CommandType.Move] = new MoveState();
        }
        public void Update()
        {
            if (_stateMachine.IsDone == true)
            {
                _commandAI.Execute(_context);
                var nextState = _stateByOrder[_context.Command];
                _stateMachine.SetAIState(nextState, _context);
            }

            _stateMachine.Update(_context);
        }

        public void Enable()
        {
            _commandAI.SetActive(true);
        }

        public void Disable()
        {
            _commandAI.SetActive(false);
        }

        public void SetActionReceiver(IActionReceiver receiver)
        {
            _context.ActionReceiver = receiver;
        }

        public void SetMovementReceiver(IMovementReceiver receiver)
        {
            _context.MovementReceiver = receiver;
        }
    }
}
