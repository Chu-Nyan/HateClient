using SAB.Cutscene;
using SAB.DataManger;
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

        cutscene.CutsceneStopped += Stop;
    }

    public void Play(string name)
    {
        var data = DataBase.Instance.CutSceneRepo.DataByName[name];
        _input.SetActive(false);
        _agent.Player.SetActive(!data.LockPlayer);
        _cutscene.Play(data);
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
