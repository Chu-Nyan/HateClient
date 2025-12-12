using ChampagneSupernova.Library.BehaviorTree;
using Chu.Utility;
using SAB.AI.Brain;
using SAB.AI.Data;

namespace SAB.AI
{
    /// <summary>
    /// AI를 단계적으로 생성
    /// </summary>
    public class AIGenerator : Singleton<AIGenerator>
    {
        public AIStrategy TempGenerate()
        {
            var ai = new BehaviorAI<AIContext>();
            ai.Init(new Selector<AIContext>());

            var condition_1 = new Leaf<AIContext>(AIConditionService.Function[ConditionCheckType.FindTarget]);
            var selector_2 = new Selector<AIContext>();
            ai.AddRootChild(condition_1);
            ai.AddRootChild(selector_2);
            
            var sequence_2_1 = new Sequence<AIContext>();
            selector_2.AddNode(sequence_2_1);

            var condition_2_1_1 = new Leaf<AIContext>(AIConditionService.Function[ConditionCheckType.CanPatrol]);
            var command_2_1_2 = new Leaf<AIContext>(AICommandService.Function[CommandType.Patrol]);
            sequence_2_1.AddNode(condition_2_1_1);
            sequence_2_1.AddNode(command_2_1_2);

            var command2_2 = new Leaf<AIContext>(AICommandService.Function[CommandType.Idle]);
            selector_2.AddNode(command2_2);
            ai.IsActivation = true;

            var brain = new AIStrategy(ai);

            return brain;
        }
    }
}
