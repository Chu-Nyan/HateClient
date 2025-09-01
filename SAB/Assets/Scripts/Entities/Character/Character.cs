using Chu.Collision;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _nav;
    private NyanCollider _collider;

    private IMovementStrategy _movementController;

    public void SetMovementStratrgy(IMovementStrategy movement)
    {
        _movementController = movement;
        _movementController.Init(_nav);
    }

    public void FixedUpdate()
    {
        if (_movementController.IsMoving == true)
        {
            _movementController.Move();
        }
        _movementController.Move();
    }
}
