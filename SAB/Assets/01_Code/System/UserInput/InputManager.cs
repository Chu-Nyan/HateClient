using Chu.Utility;
using UnityEngine;

namespace SAB.GameSystem
{
    public class InputManager : Singleton<InputManager>
    {
        private readonly InputSystem_Actions _action;

        public readonly InputEvent<Vector2> MousePoint;
        public readonly InputEvent<Vector2> WASD;
        public readonly InputEvent LeftClick;

        private Vector2 _mousePosition;

        public Vector2 MousePosition
        {
            get => _mousePosition;
        }

        public InputManager()
        {
            _action = new();
            MousePoint = new(_action.Player.MousePosition, InputEventType.Performed);
            MousePoint.RegisterPerformed(value => _mousePosition = value);

            WASD = new(_action.Player.WASD, InputEventType.Performed | InputEventType.Canceled);
            LeftClick = new(_action.Player.Click, InputEventType.Performed);
        }

        public void SetActive(bool value)
        {
            if (value == true)
                _action.Enable();
            else
                _action.Disable();
        }
    }
}
