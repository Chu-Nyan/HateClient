using Chu;
using SAB.DataManger;
using SAB.EntityAgent.AI;
using SAB.Item;
using SAB.Unit.Combat;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;
    private TopViewCamera _topViewCam;
    private Character _player;
    private UnitController _unitController;

    private void Awake()
    {
        StartChuEngine();

        GenerateStatic();
        GenerateInstance();

        InitStatic();
        InitInstance();
    }

    private void StartChuEngine()
    {
        var engine = new ChuEngine(gameObject);
        engine.ActivateCollisionSystem(new(-100, 100, -100, 100), 20);
    }

    private void GenerateStatic()
    {
        new DataBase();
        new InputManager();

        new AIGenerator();
        new SkillObjectFactory();
        new CharacterGenerator(DataBase.Instance);
        new SkillGenerator(DataBase.Instance);
        new ItemFactory(DataBase.Instance);
        new UIManager();
        StartCoroutine(LoadMap("Scene_Forest"));
    }

    private void GenerateInstance()
    {
        _topViewCam = new TopViewCamera();
        _unitController = new(transform);
    }

    private void InitStatic()
    {
        InputManager.Instance.SetActive(true);
        InitText("en");
    }

    private void InitInstance()
    {
        _topViewCam.InitCamera(_camera);
    }

    private void GameStart()
    {
        SetPracticeScene();
    }

    private void InitText(string lang)
    {
        new TextResource<TextID>();
        TextResource<TextID>.Instance.LoadTexts(lang);
    }

    private void SetPracticeScene()
    {
        _player = _unitController.GenerateCharacter(1, new Vector3(100, 0, 100), new CustomizingData(1, 1, 1, 1));
        _unitController.BindRecevier(_player, UnitController.Oner.Player, true);

        var weapon = ItemFactory.Instance.GenerateItem(1);
        var armor = ItemFactory.Instance.GenerateItem(7);
        var shield = ItemFactory.Instance.GenerateItem(16);

        _player.Equip(weapon as IHasEquipmentData);
        _player.Equip(armor as IHasEquipmentData);
        _player.Equip(shield as IHasEquipmentData);

        var _npc = _unitController.GenerateCharacter(2, new Vector3(101, 0, 101), new CustomizingData(2, 1, 1, 1));
        _unitController.BindRecevier(_npc, UnitController.Oner.AI, true);

        _topViewCam.StickCameraArm(_player.transform);
    }

    private IEnumerator LoadMap(string name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        yield return op.isDone; // 🔥 로드 완료까지 대기
        GameStart(); // 여기서 안전하게 시작
    }
}
