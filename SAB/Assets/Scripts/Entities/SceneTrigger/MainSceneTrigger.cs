using Chu.Core;
using SAB.Cutscene;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.EntityAgent.AI;
using SAB.Facade;
using SAB.Unit.Combat;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _topviewCam;
    private TopViewCamera _topViewCam;

    private MapReferenceHub _currentMapReference;
    private EntityContainer _entityContainer;

    private CutsceneMapTrigger _cutscenePlayer;
    private AgentController _agentController;
    private CutsceneController _cutsceneDirector;

    // Facade
    private GamePlayFacade _gamePlayFacade;
    [SerializeField]
    private WorldProfile _worldProfile;

    private void Awake()
    {
        StartChuEngine();

        GenerateStatic();
        GenerateInstance();
        GenerateFacade();

        InitStatic();
        InitInstance();


        StartCoroutine(ChangeMap(MapType.Forest));
    }

    private void StartChuEngine()
    {
        var engine = new ChuEngine(gameObject, "en");
        engine.ActivateCollisionSystem(new(0, 200, 0, 200), 20);
    }

    private void GenerateStatic()
    {
        new DataBase();
        new InputManager();

        new AIGenerator();
        new SkillObjectFactory();
        new CharacterFactory(DataBase.Instance);
        new SkillGenerator(DataBase.Instance);
        new ItemFactory(DataBase.Instance);
        new UIManager();

        _cutsceneDirector = AssetManager.GenerateLoadAssetSync<CutsceneController>(Const.Asset_CutsceneManger, "CutsceneManager", transform);
    }

    private void GenerateInstance()
    {
        _topViewCam = new TopViewCamera();
        _entityContainer = new();
        _cutscenePlayer = new();
        _agentController = gameObject.AddComponent<AgentController>();
    }

    private void InitStatic()
    {
        InputManager.Instance.SetActive(true);
        _cutsceneDirector.Init(Camera.main.GetComponent<CinemachineBrain>());
        SkillObjectFactory.Instance.Init(_worldProfile.FactionTable);
        CharacterFactory.Instance.Init(_agentController, _entityContainer);
    }

    private void InitInstance()
    {
        _topViewCam.InitCamera(_topviewCam);
        _cutscenePlayer.Init(_gamePlayFacade.Cutscene.Play);
    }

    private void GenerateFacade()
    {
        _gamePlayFacade = new(_cutsceneDirector, _agentController, _entityContainer);
    }

    private void GameStart()
    {
        SetPracticeScene();
    }

    private void SetPracticeScene()
    {
        var player = CharacterFactory.Instance.Create(BrainType.Player, 1, new Vector3(100, 0, 100), Quaternion.identity, UniqueEntityType.Player);
        var _npc = CharacterFactory.Instance.Create(BrainType.AI, 10, new Vector3(101, 0, 101), Quaternion.identity);
        _topViewCam.StickCameraArm(player.transform);
    }

    private IEnumerator ChangeMap(MapType type)
    {
        string sceneName = $"Scene_{type}";
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return op.isDone;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        _currentMapReference = SetLoadMapReference(loadedScene);
        _currentMapReference.InitObject();
        _cutscenePlayer.SetupMap(type);
        GameStart();
    }

    private MapReferenceHub SetLoadMapReference(Scene scene)
    {
        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            if (rootObj.CompareTag(Const.Tag_MapReferenceHub) == false)
                continue;

            return rootObj.GetComponent<MapReferenceHub>();
        }

        throw new System.Exception("Map scene is missing a hub.");
    }
}
