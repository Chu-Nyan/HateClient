using UnityEngine;

public class GameSceneTrigger : MonoBehaviour
{
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
    }

    private void GenerateInstance()
    {

    }

    private void InitStatic()
    {

    }

    private void InitInstance()
    {

    }

    private void GameStart()
    {

    }
}
