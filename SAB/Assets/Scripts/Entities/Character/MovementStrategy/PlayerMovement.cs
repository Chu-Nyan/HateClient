using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : IMovementStrategy
{
    private Transform _transform;
    private NavMeshAgent _agent;
    private Vector2 _direction;

    public bool IsMoving
    {
        get => _direction != Vector2.zero;
    }

    public void Init(NavMeshAgent agent)
    {
        _transform = agent.transform;
        _agent = agent;
        InputManager.Instance.RegisterWASDPerformed(SetDirection);
        InputManager.Instance.RegisterWASDCanceled(SetDirection);
    }

    public void Move()
    {
        var destination = new Vector3(_transform.position.x + _direction.x, _transform.position.y, _transform.position.z + _direction.y);
        _agent.SetDestination(destination);
    }

    private void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    public void Reset()
    {
        InputManager.Instance.UnregisterWASDPerformed(SetDirection);
    }
}
