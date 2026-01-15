using SAB.EntityAgent.AI.Context;
using UnityEngine;

namespace SAB.EntityAgent.AI.StateMachine
{
    /// <summary>
    /// 대기 명령
    /// </summary>
    public class IdleCommand : ICommandState<AIContext>
    {
        private float _duration;
        private bool _done;

        public bool IsDone
        {
            get => _done;
        }

        public void Enter(AIContext data)
        {
            _done = false;
            _duration = 0;
        }

        public void Exit(AIContext data)
        {
        }

        public void Update(AIContext data)
        {
            _duration += Time.deltaTime;
            if (_duration >= data.IdleCommandData.Time)
            {
                _done = true;
            }
        }
    }
}
