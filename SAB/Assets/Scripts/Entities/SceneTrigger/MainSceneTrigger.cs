using UnityEngine;

public class GameSceneTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;
    private TopViewCamera _topViewCam;
    private Character _player; 

    private void Awake()
    {
        GenerateStatic();
        GenerateInstance();

        InitStatic();
        InitInstance();
        GameStart();
    }

    private void GenerateStatic()
    {
        new InputManager();
        InputManager.Instance.SetActive(true);
        InitText("en");
    }

    private void GenerateInstance()
    {
        SetPracticeScene();
        _topViewCam = new TopViewCamera();
    }

    private void InitStatic()
    {

    }

    private void InitInstance()
    {
        SetCameraSetting();
    }

    private void GameStart()
    {

    }

    private void InitText(string lang)
    {
        new TextResource<TextID>();
        TextResource<TextID>.Instance.LoadTexts(lang);
    }

    private void SetCameraSetting()
    {
        _topViewCam.InitCamera(_camera);
        _topViewCam.StickCameraArm(_player.transform);
    }

    private void SetPracticeScene()
    {
        _player = AssetManager.GenerateLoadAssetSync<Character>(Const.Asset_Character);
        _player.SetMovementStratrgy(new PlayerMovement());
    }
}
