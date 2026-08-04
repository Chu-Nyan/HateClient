using Chu.Core;
using SAB.Cutscene;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.EntityAgent.AI;
using SAB.Facade;
using SAB.Skill;
using SAB.UI;
using SAB.Unit;
using Unity.Cinemachine;
using UnityEngine;

namespace SAB.GameSystem
{
    public class GameSceneTrigger : MonoBehaviour
    {
        private const string CutsceneMangerAssetPath = "CutSceneManager";

        [SerializeField]
        private GameObject _topviewCam;
        private TopViewCamera _topViewCam;

        private EntityContainer _entityContainer;

        private CutsceneMapTrigger _cutscenePlayer;
        private AgentController _agentController;
        private CutsceneController _cutsceneDirector;

        // Facade
        private GamePlayFacade _gamePlayFacade;

        private void Awake()
        {
            ChuEngine.Run(new EngineSetting("en", new(0, 300, 0, 300), 4));

            GenerateStatic();
            GenerateInstance();

            InitStatic();
            InitInstance();

            GenerateFacade();
            InitFacade();

            GameStart();
        }

        private void GenerateStatic()
        {
            new DataBase();
            new InputManager();
            new AIGenerator();
            new CharacterFactory(DataBase.Instance);
            new SkillFactory(DataBase.Instance.SkillRepo, DataBase.Instance.WorldProfile.FactionTable);
            new ItemFactory(DataBase.Instance);
            new UIManager();
        }

        private void GenerateInstance()
        {
            _topViewCam = new TopViewCamera();
            _entityContainer = new();
            _cutscenePlayer = new();
            _agentController = gameObject.AddComponent<AgentController>();
            _cutsceneDirector = AssetManager.InstantiateLoadAssetSync<CutsceneController>(CutsceneMangerAssetPath, "CutsceneManager", transform);
        }

        private void InitStatic()
        {
            InputManager.Instance.SetActive(true);
            CharacterFactory.Instance.Init(_agentController, _entityContainer);
        }

        private void InitInstance()
        {
            _topViewCam.InitCamera(_topviewCam);
            _cutsceneDirector.Init(Camera.main.GetComponent<CinemachineBrain>());
        }

        private void GenerateFacade()
        {
            var changeMap = new MapFacade(_cutscenePlayer, _topViewCam);
            var cutscene = new CutsceneFacade(_cutsceneDirector, _agentController, _entityContainer);
            _gamePlayFacade = new(cutscene, changeMap);
        }

        private void InitFacade()
        {
            _cutscenePlayer.RegisterTriggerEnterAction(_gamePlayFacade.Cutscene.Play);
        }

        private void GameStart()
        {
            _gamePlayFacade.Map.Change(MapType.Forest, 0);
        }
    }
}
