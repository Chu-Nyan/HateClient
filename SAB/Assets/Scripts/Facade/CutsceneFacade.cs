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

        cutscene.CutsceneStarted += Ready;
        cutscene.CutsceneStopped += Stop;
    }

    public void Play(int id)
    {
        Ready(id);
        _cutscene.Play(id);
    }

    private void Ready(int id)
    {
        var data = _cutscene.GetCutsceneData(id);
        _input.SetActive(false);
        if (data.LockPlayer == true)
        {
            _agent.Player.SetActive(false);
        }
    }

    public void Stop(int id)
    {
        var data = _cutscene.GetCutsceneData(id);

        _input.SetActive(true);
        if (data.LockPlayer == true)
        {
            _agent.Player.SetActive(true);
        }
    }
}
