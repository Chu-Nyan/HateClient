using SAB.Unit.Combat;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격을 입력을 받을 수 있음
/// </summary>
public interface IOffenseReceiver : IInputReceiver
{
    public Transform transform { get; }
    public List<Skill> Skills { get; } 
    public void Attack(int skillIndex, Vector3 targetPoint);
}
