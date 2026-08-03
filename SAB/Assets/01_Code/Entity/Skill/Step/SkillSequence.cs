using System.Collections.Generic;
using UnityEngine;

namespace SAB.Skill
{
    /// <summary>
    /// 스킬의 작동 순서
    /// </summary>
    public class SkillSequence
    {
        private readonly AttackContext _context;
        private readonly List<ISkillStep> _steps;

        private int _currentStepIndex;
        private bool _isDone;

        public AttackContext Context
        {
            get => _context;
        }

        public SkillSequence(AttackContext context, List<ISkillStep> steps)
        {
            _context = context;
            _steps = steps;
            _currentStepIndex = 0;
            _isDone = false;
        }

        public bool TickAndCheck(IHasStats stats)
        {
            if (_steps == null || _steps.Count == 0)
            {
                Debug.LogError("SkillStep이 존재하지 않음");
                return true;
            }
            if (_isDone == true)
            {
                Debug.LogWarning("완료된 Sequence 진입");
                return true;
            }

            while (_currentStepIndex < _steps.Count)
            {
                var step = _steps[_currentStepIndex];

                if (step.IsDone == false)
                    step.Tick(stats);

                if (step.IsDone == true)
                    _currentStepIndex++;
                else
                    return false;
            }

            _isDone = true;
            return true;
        }
    }
}
