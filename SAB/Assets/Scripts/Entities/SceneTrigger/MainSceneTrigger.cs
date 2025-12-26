using Chu;
using SAB.AI;
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
        engine.ActivateCollisionSystem(new(-100,100,-100,100),20);
    }

    private void GenerateStatic()
    {
        new InputManager();
        new AIGenerator();
        new ProjectileGenerator();
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
        _player = _unitController.GenerateCharacter(UnitController.Oner.Player, new Vector3(50,0,50));
        _unitController.BindUnitHandler(_player, UnitController.Oner.Player);
        _unitController.ToggleUnitHandler(_player.ReceiverID, true);

        var _npc = _unitController.GenerateCharacter(UnitController.Oner.AI, new Vector3(50, 0, 50));
        _unitController.BindUnitHandler(_npc, UnitController.Oner.AI);
        _unitController.ToggleUnitHandler(_npc.ReceiverID, true);

        _topViewCam.StickCameraArm(_player.transform);

    }
}
