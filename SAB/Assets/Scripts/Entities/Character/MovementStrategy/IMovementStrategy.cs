using UnityEngine.AI;

/// <summary>
/// 행동을 제어할 기술을 정의하는 전략 패턴 인터페이스
/// </summary>
public interface IMovementStrategy
{
    public void Init(NavMeshAgent agent);
    public void Move();
    public bool IsMoving { get; }
}
