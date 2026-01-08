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
        }

        public void Tick(IHasStats stats)
        {
        }
    }
}
