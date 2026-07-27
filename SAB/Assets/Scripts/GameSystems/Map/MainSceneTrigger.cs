using Chu.Core;
using SAB.Cutscene;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.EntityAgent.AI;
using SAB.Facade;
using SAB.Skill;
using SAB.UI;
using SAB.Unit;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SAB.GameSystem
{
    public class GameSceneTrigger : MonoBehaviour
    {
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
            _cutsceneDirector = AssetManager.GenerateLoadAssetSync<CutsceneController>(Const.Asset_CutsceneManger, "CutsceneManager", transform);
        }

        private void InitStatic()
        {
            InputManager.Instance.SetActive(true);
            CharacterFactory.Instance.Init(_agentController, _entityContainer);
        }

        private void InitInstance()
        {
            _topViewCam.InitCamera(_topviewCam);
            _cutscenePlayer.Init(_gamePlayFacade.Cutscene.Play);
            _cutsceneDirector.Init(Camera.main.GetComponent<CinemachineBrain>());
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
            var mapDef = DataBase.Instance.MapRepo.DataByType[type];
            foreach (var item in mapDef.Characters)
            {
                var request = item.Value;
                var acter = CharacterFactory.Instance.Create(request.BrainType, request.UnitID, request.Position, request.Rotation);
                if (mapDef.Favorites.TryGetValue(item.Key, out var id) == true)
                {
                    _entityContainer.AddFavoriteObject(id, acter);
                }
            }

            _cutscenePlayer.SetupMap(mapDef.CutsceneTriggers);
            GameStart();
        }
    }
}
