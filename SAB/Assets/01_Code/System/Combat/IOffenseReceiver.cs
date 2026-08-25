using SAB.EntityAgent;
using SAB.Skill;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.GameSystem
{
    /// <summary>
    /// 공격을 입력을 받을 수 있음
    /// </summary>
    public interface IOffenseReceiver : IInputReceiver
    {
        public Transform transform { get; }
        public int InstanceID { get; }
        public List<SkillKernel> Skills { get; }
        public void Attack(int skillIndex, Vector3 targetPoint);
        public void SetCombatMode(bool value);
    }
}
