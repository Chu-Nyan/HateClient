using System;

namespace SAB.Unit.Combat
{
    public class InstantSkillStep : ISkillStep
    {
        private InstanceStepData _data;
        private AttackContext _context;
        private bool _isDone;

        public bool IsDone
        {
            get => _isDone;
        }

        public void Refresh(IStepData data, AttackContext context)
        {
            if (data is not InstanceStepData stepData)
                throw new Exception("잘못된 SkillStep 초기화");

            _data = stepData;
            _context = context;
        }

        public void Tick(IHasStats stats)
        {
            stats.AddHP(-(_context.Damage * _data.DamageRate));
            _isDone = true;
        }
    }
}
