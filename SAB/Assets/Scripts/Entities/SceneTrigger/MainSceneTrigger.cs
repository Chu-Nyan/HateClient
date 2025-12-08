using Chu;
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
    }

    private void GenerateStatic()
    {
        new InputManager();
    }

    private void GenerateInstance()
    {
        _topViewCam = new TopViewCamera();
        _unitController = new();
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
        _player = _unitController.GenerateCharacter(UnitController.Oner.Player);
        _topViewCam.StickCameraArm(_player.transform);

    }
}
