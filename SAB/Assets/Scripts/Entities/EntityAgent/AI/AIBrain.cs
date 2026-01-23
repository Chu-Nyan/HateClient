using ChampagneSupernova.Library.BehaviorTree;
using SAB.EntityAgent.AI.Context;
using SAB.EntityAgent.AI.StateMachine;
using System.Collections.Generic;
using UnityEngine;

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
        private readonly CombatModeDecider _combatModeDecider;
        private AIContext _context;

        public AIBrain(BehaviorAI<AIContext> brain)
        {
            _decider = brain;

            _executor = new();
            _context = new();
            _stateByCommand = new();

            _stateByCommand[CommandType.Idle] = new IdleCommand();
            _stateByCommand[CommandType.Move] = new MoveCommand();
            _combatModeDecider = new CombatModeDecider();
        }

        public void Tick()
        {
            if (_context.CombatReceiver != null)
            {
                float time = Time.deltaTime;
                _combatModeDecider.TickForExit(time);
            }
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
            if (_context.CombatReceiver == null && _context.MovementReceiver == null)
                return;

            _decider.SetActive(true);
        }

        public void Disable()
        {
            _decider.SetActive(false);
        }

        public void SetCombatReceiver(IOffenseReceiver receiver)
        {
            _context.CombatReceiver = receiver;
            _combatModeDecider.Setup(receiver.transform, receiver.InstanceID);
        }

        public void SetMovementReceiver(IMovementReceiver receiver)
        {
            _context.MovementReceiver = receiver;
        }
    }
}
