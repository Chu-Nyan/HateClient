using SAB.Cutscene;
using SAB.EntityAgent;

namespace SAB.Facade
{
    public class GamePlayFacade
    {
        public CutsceneFacade Cutscene;

        public GamePlayFacade(CutsceneController cutscene, AgentController agent)
        {
            Cutscene = new(cutscene, agent, InputManager.Instance);
        }
    }
}
