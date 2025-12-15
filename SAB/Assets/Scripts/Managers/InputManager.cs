using Chu.Utility;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private readonly InputSystem_Actions _action;

    private event Action<Vector2> WASDPerformed;
    private event Action<Vector2> WASDCanceled;

    private event Action<Vector2> LeftClicked;

    private Vector2 _mousePosition;

    public Vector2 MousePosition
    {
        get => _mousePosition;
    }

    public InputManager()
    {
        _action = new();
        _action.Player.WASD.performed += OnWASDPerformed;
        _action.Player.WASD.canceled += OnWASDCanceled;
        _action.Player.Click.performed += OnLeftClicked;
        _action.Player.MousePosition.performed += GetMousePosition;
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

    public void RegisterLeftClickedPerformed(Action<Vector2> action)
    {
        LeftClicked += action;
    }

    public void UnregisterWASDPerformed(Action<Vector2> action)
    {
        WASDPerformed -= action;
    }

    public void UnregisterWASDCanceled(Action<Vector2> action)
    {
        WASDCanceled -= action;
    }

    public void UnregisterLeftClickedPerformed(Action<Vector2> action)
    {
        LeftClicked -= action;
    }

    private void OnWASDPerformed(InputAction.CallbackContext value)
    {
        WASDPerformed?.Invoke(value.ReadValue<Vector2>());
    }

    private void OnWASDCanceled(InputAction.CallbackContext value)
    {
        WASDCanceled?.Invoke(value.ReadValue<Vector2>());
    }

    private void OnLeftClicked(InputAction.CallbackContext value)
    {
        LeftClicked?.Invoke(_mousePosition);
    }

    private void GetMousePosition(InputAction.CallbackContext value)
    {
        _mousePosition = value.ReadValue<Vector2>();
    }
    #endregion
}
