using Chu;
using SAB.EntityAgent.AI;
using SAB.Unit.Combat;
using UnityEngine;

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
        GameStart();
    }

    private void StartChuEngine()
    {
        var engine = new ChuEngine(gameObject);
        engine.ActivateCollisionSystem(new(-100, 100, -100, 100), 20);
    }

    private void GenerateStatic()
    {
        new InputManager();
        new AIGenerator();
        new ProjectileGenerator();
        new SkillGenerator();
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
        _player = _unitController.GenerateCharacter(UnitType.Human, new Vector3(50, 0, 50));
        _unitController.BindRecevier(_player, UnitController.Oner.Player, true);

        var _npc = _unitController.GenerateCharacter(UnitType.Mimic, new Vector3(50, 0, 50));
        _unitController.BindRecevier(_npc, UnitController.Oner.AI, true);

        _topViewCam.StickCameraArm(_player.transform);
    }
}
