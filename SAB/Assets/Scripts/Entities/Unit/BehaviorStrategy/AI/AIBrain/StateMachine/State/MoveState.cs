using SAB.AI.Data;
using UnityEngine;

namespace SAB.AI.Brain
{
    /// <summary>
    /// 목표 지점으로 이동 명령
    /// </summary>
    public class MoveState : IMachineState<AIContext>
    {
        private static float _stoppingDistance = 0.05f;
        private bool _done;

        public bool IsDone
        {
            get => _done;
        }

        public void Enter(AIContext context)
        {
            if (context.MovementReceiver == null)
                throw new System.Exception("Receiver is null");

            _done = false;
            var destination = context.MoveCommandData.Destination;
            context.MovementReceiver.SetDestination(destination);
        }

        public void Update(AIContext context)
        {
            var remainDistance = Vector3.Distance(context.MovementReceiver.transform.position, context.MoveCommandData.Destination);
            if (remainDistance <= _stoppingDistance)
            {
                _done = true;
            }
            // TODO : 움직임이 취소되어야 할 경우 체크
        }

        public void Exit(AIContext context)
        {
            // TODO : 이동 중이면 움직임 취소
        }
    }
}
