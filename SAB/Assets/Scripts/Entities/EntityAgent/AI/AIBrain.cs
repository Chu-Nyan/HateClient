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
        private readonly Dictionary<CommandType, ICommandState<AIContext>> _stateByCommand;
        private readonly BehaviorAI<AIContext> _decider;
        private readonly CommandExecutor _executor;
        private AIContext _context;

        public AIBrain(BehaviorAI<AIContext> brain)
        {
            _decider = brain;

            _executor = new();
            _context = new();
            _stateByCommand = new();

            _stateByCommand[CommandType.Idle] = new IdleCommand();
            _stateByCommand[CommandType.Move] = new MoveCommand();
        }
        public void Tick()
        {
            if (_executor.IsDone == true)
            {
                _decider.Execute(_context);
                var nextState = _stateByCommand[_context.Command];
                _executor.SetAIState(nextState, _context);
            }

            _executor.Tick(_context);
        }

        public void Enable()
        {
            _decider.SetActive(true);
        }

        public void Disable()
        {
            _decider.SetActive(false);
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
