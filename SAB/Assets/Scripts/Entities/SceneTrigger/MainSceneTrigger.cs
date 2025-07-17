using System.Collections;
using System.ComponentModel;
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
        InitText("en");
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

    private void InitText(string lang)
    {
        new TextResource<TextID>();
        TextResource<TextID>.Instance.LoadTexts(lang);
    }
}
