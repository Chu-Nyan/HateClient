using UnityEngine;

/// <summary>
/// 공격을 입력을 받을 수 있음
/// </summary>
public interface ICombatReceiver : IInputReceiver
{
    public Transform transform { get; }
    public void Attack(Vector3 dir);
}
