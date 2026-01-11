using System;
using UnityEngine;

namespace SAB.Unit.Combat
{
    public class DoTSkillStep : ISkillStep
    {
        private DotStepData _data;
        private AttackContext _context;
        private bool _isDone;
        private float _remainInterval;
        private float _remainTick;

        public bool IsDone
        {
            get => _isDone;
        }

        public void Refresh(IStepData data, AttackContext context)
        {
            if (data is not DotStepData stepData)
                throw new Exception("잘못된 SkillStep 초기화");

            _data = stepData;
            _context = context;
            _isDone = false;
            _remainInterval = DotStepData.DamageInterval;
            _remainTick = Mathf.FloorToInt(_data.Duration / DotStepData.DamageInterval);
        }

        public void Tick(IHasStats defensive)
        {
            _remainInterval -= Time.deltaTime;
            if (_remainInterval <= 0)
            {
                _remainTick--;
                _remainInterval += DotStepData.DamageInterval;
                defensive.HP -= _context.Damage * _data.DamageRate;

                if (_remainTick == 0)
                    _isDone = true;
            }
        }
    }
}
