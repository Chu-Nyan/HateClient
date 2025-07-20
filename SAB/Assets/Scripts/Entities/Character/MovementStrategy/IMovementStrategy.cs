using UnityEngine.AI;

public interface IMovementStrategy
{
    public void Init(NavMeshAgent agent);
    public void Move();
    public bool IsMoving { get; }
}
