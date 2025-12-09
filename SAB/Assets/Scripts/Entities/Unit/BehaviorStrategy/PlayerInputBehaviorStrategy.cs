using UnityEngine;

/// <summary>
/// 플레이어의 입력으로 Unit 행동을 제어함
/// </summary>
public class PlayerInputBehaviorStrategy : IUnitBehaviorStrategy
{
    private IMovementReceiver _movementReceiver;
    private IActionReceiver _actionReceiver;
    private Vector2 _direction;
    private bool _isMoving;


    public void Update()
    {
        if (_isMoving == true)
            MoveDirection();
    }

    public void SetMovementReceiver(IMovementReceiver receiver)
    {
        _movementReceiver = receiver;
    }

    public void SetActionReceiver(IActionReceiver receiver)
    {
        _actionReceiver = receiver;
    }

    public void Enable()
    {
        InputManager.Instance.RegisterWASDPerformed(SetDiection);
        InputManager.Instance.RegisterWASDCanceled(SetDiection);
    }

    public void Disable()
    {
        InputManager.Instance.UnregisterWASDPerformed(SetDiection);
        InputManager.Instance.UnregisterWASDCanceled(SetDiection);
    }


    private void SetDiection(Vector2 dir)
    {
        _direction = dir;
        _isMoving = _direction != Vector2.zero;
    }

    private void MoveDirection()
    {
        var direction = new Vector3(_direction.x * 0.1f, 0, _direction.y * 0.1f);
        _movementReceiver.Move(direction);
    }
}
