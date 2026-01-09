using System;
using UnityEngine;

namespace SAB.Unit.Combat
{
    public class TimerSkillStep : ISkillStep
    {
        private TimerStepData _data;
        private float _remainTime;
        private bool _isDone;

        public bool IsDone
        {
            get => _isDone;
        }

        public void Refresh(IStepData data, AttackContext context)
        {
            if (data is not TimerStepData stepData)
                throw new Exception("잘못된 SkillStep 초기화");

            _data = stepData;
            _remainTime = _data.Duration;
            _isDone = false;
        }

        public void Tick(IHasStats stats)
        {
            _remainTime -= Time.deltaTime;

            if (_remainTime <= 0f)
                _isDone = true;
        }
    }
}
