using SAB.Cutscene;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.Unit;

public class CutsceneFacade
{
    private CutsceneController _cutscene;
    private AgentController _agent;
    private InputManager _input;
    private EntityContainer _entityContainer;

    public CutsceneFacade(CutsceneController cutscene, AgentController agent, InputManager input, EntityContainer entityContainer)
    {
        _cutscene = cutscene;
        _agent = agent;
        _input = input;
        _entityContainer = entityContainer;

        cutscene.CutsceneStopped += Stop;
    }

    public void Play(string name)
    {
        var data = DataBase.Instance.CutSceneRepo.DataByName[name];
        _input.SetActive(false);
        _agent.Player.SetActive(!data.LockPlayer);
        PrepareCharacter(data);
        _cutscene.Play(data);
    }

    private void PrepareCharacter(CutsceneData cutsceneData)
    {
        if (cutsceneData.SpawnContainer.TryGetTable<SpawnRequest>(out var datas) == true)
        {
            foreach (var item in datas)
            {
                var acterData = item.Value;
                var acter = CharacterFactory.Instance.Create(BrainType.None, acterData.ID, acterData.Position, acterData.Rotation);
                _cutscene.RegisterExternalObject(item.Key, acter);
            }
        }
        foreach (var item in cutsceneData.SceneObjectBindingIDs)
        {
            var obj = _entityContainer.ObjectByFavorite[item.Value.ToString()];
            _cutscene.RegisterExternalObject(item.Key, (ICutsceneObject)obj);
        }
        foreach (var item in cutsceneData.BindingSlots)
        {
            _cutscene.RegisterExternalObject(item.Key, (ICutsceneObject)_entityContainer.GetUniqueEntity(item.Value));
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
