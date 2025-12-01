using UnityEngine;

/// <summary>
/// 입력을 받아 움직이는 객체
/// </summary>
public interface IMovementReceiver
{
    void Move(Vector3 dir);
}
