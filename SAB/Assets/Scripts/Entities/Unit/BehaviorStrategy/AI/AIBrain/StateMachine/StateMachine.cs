using SAB.AI.Data;

namespace SAB.AI.Brain
{
    /// <summary>
    /// 받은 명령으로 IInputReceiver 제어
    /// </summary>
    public class StateMachine
    {
        private IMachineState<AIContext> _currentState;

        public bool IsDone
        {
            get => _currentState == null || _currentState.IsDone;
        }

        public void SetAIState(IMachineState<AIContext> state, AIContext context)
        {
            _currentState?.Exit(context);
            _currentState = state;
            _currentState.Enter(context);
        }

        public void Update(AIContext context)
        {
            _currentState?.Update(context);
        }

        public bool TryEnd(AIContext context)
        {
            if (_currentState != null && _currentState.IsDone == true)
            {
                _currentState.Exit(context);
                _currentState = null;
                return true;
            }

            return false;
        }
    }
}
