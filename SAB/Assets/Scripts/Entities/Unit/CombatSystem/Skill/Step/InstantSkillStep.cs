using System;
using UnityEngine;

namespace SAB.Unit.Combat
{
    public class InstantSkillStep : ISkillStep
    {
        private InstanceStepData _data;
        private AttackContext _dmg;
        private bool _isDone;

        public bool IsDone
        {
            get => _isDone;
        }

        public void Refresh(IStepData data, AttackContext dmg)
        {
            if (data is not InstanceStepData instant)
                throw new Exception("잘못된 SkillStep 초기화");

            _data = instant;
            _dmg = dmg;
        }

        public void Tick(IHasStats stats)
        {
            // 공격자 데미지와 계산식
            Debug.Log($"{stats.HP} - {_dmg}피해를 입힘");
            _isDone = true;
        }
    }
}
