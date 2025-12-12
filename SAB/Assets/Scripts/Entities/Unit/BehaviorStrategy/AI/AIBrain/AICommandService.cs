using ChampagneSupernova.Library.BehaviorTree;
using SAB.AI.Data;
using System;
using System.Collections.Generic;

namespace SAB.AI.Brain
{
    /// <summary>
    /// AI가 명령 전달에 필요한 함수 모음
    /// </summary>
    public static class AICommandService
    {
        public static Dictionary<CommandType, Func<AIContext, MethodResult>> Function = new()
        {
            {CommandType.Idle,  Idle},
            {CommandType.Patrol, Patrol}
        };

        public static MethodResult Move(AIContext context)
        {
            context.OrderToMove(context.MoveCommandData);

            return MethodResult.Success;
        }

        public static MethodResult Idle(AIContext context)
        {
            var waitTime = context.MovementReceiver.IdleData.WaitTime;

            context.OrderToIdle(new IdleCommandData(waitTime));
            return MethodResult.Success;
        }

        public static MethodResult Patrol(AIContext context)
        {
            var data = context.MovementReceiver.PatrolData;
            var x = UnityEngine.Random.Range(data.XRange.x, data.XRange.y);
            var z = UnityEngine.Random.Range(data.YRange.x, data.YRange.y);
            var pos = context.MovementReceiver.transform.position + new UnityEngine.Vector3(x, 0, z);
            context.OrderToMove(new MoveCommandData(pos));

            return MethodResult.Success;
        }
    }
}
