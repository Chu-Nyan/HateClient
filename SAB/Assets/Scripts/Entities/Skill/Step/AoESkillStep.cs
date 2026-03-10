using System;

namespace SAB.Unit.Combat
{
    public class AoESkillStep : ISkillStep
    {
        private AoEStepData _data;
        private AttackContext _context;
        private bool _isDone;

        public bool IsDone
        {
            get => _isDone;
        }

        public void Refresh(IStepData data, AttackContext context)
        {
            if (data is not AoEStepData stepData)
                throw new Exception("잘못된 SkillStep 초기화");

            _data = stepData;
            _context = context;

        }

        public void Tick(IHasStats defensive)
        {
            // TODO : 범위 스킬 로직
            _isDone = true;
        }
    }
}
