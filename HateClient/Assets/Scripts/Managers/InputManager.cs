using System;
using Library.DesignPattern;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private readonly InputSystem_Actions _action;

    private event Action<Vector2> WASDPerformed;
    private event Action<Vector2> WASDCanceled;

    public InputManager()
    {
        _action = new();
        _action.Player.WASD.performed += OnWASDPerformed;
        _action.Player.WASD.canceled += OnWASDCanceled;
    }

    public void SetActive(bool value)
    {
        if (value == true)
            _action.Enable();
        else
            _action.Disable();
    }

    #region WASD
    public void RegisterWASDPerformed(Action<Vector2> action)
    {
        WASDPerformed += action;
    }

    public void RegisterWASDCanceled(Action<Vector2> action)
    {
        WASDCanceled += action;
    }

    public void UnregisterWASDPerformed(Action<Vector2> action)
    {
        WASDPerformed -= action;
    }

    public void UnregisterWASDCanceled(Action<Vector2> action)
    {
        WASDCanceled -= action;
    }

    private void OnWASDPerformed(InputAction.CallbackContext value)
    {
        WASDPerformed?.Invoke(value.ReadValue<Vector2>());
        Debug.Log("zz");
    }

    private void OnWASDCanceled(InputAction.CallbackContext value)
    {
        WASDCanceled?.Invoke(value.ReadValue<Vector2>());
        Debug.Log("zz");
    }
    #endregion
}
