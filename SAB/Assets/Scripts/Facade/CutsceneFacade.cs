using SAB.Cutscene;
using SAB.EntityAgent;

public class CutsceneFacade
{
    private CutsceneController _cutscene;
    private AgentController _agent;
    private InputManager _input;

    public CutsceneFacade(CutsceneController cutscene, AgentController agent, InputManager input)
    {
        _cutscene = cutscene;
        _agent = agent;
        _input = input;

        cutscene.CutsceneStarted += Play;
        cutscene.CutsceneStopped += Stop;
    }

    public void Play(int id)
    {
        _input.SetActive(false);
        if (_cutscene.PlayCutsceneData.LockPlayer == true)
        {
            _agent.Player.SetActive(false);
        }
    }

    public void Stop(CutsceneData data)
    {
        _input.SetActive(true);
        if (data.LockPlayer == true)
        {
            _agent.Player.SetActive(true);
        }
    }
}
