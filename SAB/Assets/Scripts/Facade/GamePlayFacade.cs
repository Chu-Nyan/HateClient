using SAB.Cutscene;
using SAB.EntityAgent;
using SAB.GameSystem;

namespace SAB.Facade
{
    public class GamePlayFacade
    {
        public CutsceneFacade Cutscene;

        public GamePlayFacade(CutsceneController cutscene, AgentController agent, EntityContainer container)
        {
            Cutscene = new(cutscene, agent, InputManager.Instance, container);
        }
    }
}
