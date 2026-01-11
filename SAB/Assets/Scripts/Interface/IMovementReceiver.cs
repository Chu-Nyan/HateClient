using SAB.EntityAgent.AI;
using UnityEngine;

/// <summary>
/// 이동 입력을 수신
/// </summary>
public interface IMovementReceiver : IInputReceiver
{
    public Transform transform { get; }
    public IMovementAIDataView MovementAIData { get; }
    public bool IsMoving { get; }
    public void SetDestination(Vector3 dir);
    public void Move(Vector3 dir);
}
