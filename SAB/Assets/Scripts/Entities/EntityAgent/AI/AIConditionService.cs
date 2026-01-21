using ChampagneSupernova.Library.BehaviorTree;
using SAB.EntityAgent.AI.Context;
using System;
using System.Collections.Generic;

namespace SAB.EntityAgent.AI
{
    /// <summary>
    /// AI가 상황 판단에 필요한 함수 모음 
    /// </summary>
    public static class AIConditionService
    {
        public static Dictionary<ConditionCheckType, Func<AIContext, MethodResult>> Function = new()
        {
            {ConditionCheckType.FindTarget,  FindTarget},
            {ConditionCheckType.CanPatrol, CanPatrol}
        };

        public static MethodResult FindTarget(AIContext context)
        {
            // TODO : IActionReceiver에 탐색 기능 추가

            return MethodResult.Failure;
        }

        public static MethodResult CanPatrol(AIContext context)
        {
            if (context.Command != CommandType.Idle)
                return MethodResult.Failure;

            var data = PatrolData.Default;
            var x = UnityEngine.Random.Range(data.XRange.x, data.XRange.y);
            var z = UnityEngine.Random.Range(data.YRange.x, data.YRange.y);
            var pos = context.MovementReceiver.transform.position + new UnityEngine.Vector3(x, 0, z);
            context.OrderToMove(new MoveCommandData(pos));

            return MethodResult.Success;
        }
    }
}
