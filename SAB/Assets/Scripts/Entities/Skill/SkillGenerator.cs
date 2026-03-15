using Chu.Utility;
using SAB.DataManger;
using System;
using System.Collections.Generic;

namespace SAB.Unit.Combat
{
    public class SkillGenerator : Singleton<SkillGenerator>
    {
        private readonly IDNumbering _numbering;
        private readonly Dictionary<int, SkillData> _skillDataByID;

        private Dictionary<SkillStepType, Func<ISkillStep>> _generateFuncByProcessType;

        public SkillGenerator(DataBase db) : base()
        {
            _numbering = new();
            _skillDataByID = db.SkillRepo.SkillByID;

            InitGenerateFunc();
        }

        private void InitGenerateFunc()
        {
            _generateFuncByProcessType = new()
            {
                { SkillStepType.Instant, () => new InstantSkillStep() },
                { SkillStepType.DoT, () => new DoTSkillStep() },
                { SkillStepType.AoE, () => new AoESkillStep() },
                { SkillStepType.Timer, () => new TimerSkillStep() },
            };
        }

        public Skill GetSkill(int id)
        {
            var skill = new Skill(_numbering.GetID());
            skill.Setup(_skillDataByID[id]);
            return skill;
        }

        public SkillSequence GenerateSequence(AttackContext context, HitResult hit)
        {
            var onHitStep = context.SkillData.CollisionLogics[hit.LogicID].OnHitSteps;
            var list = new List<ISkillStep>();
            for (int i = 0; i < onHitStep.Length; i++)
            {
                list.Add(GenerateStep(onHitStep[i], context));
            }

            var sequence = new SkillSequence();
            sequence.Refresh(context, list);

            return sequence;
        }

        private ISkillStep GenerateStep(IStepData data, AttackContext context)
        {
            var type = (SkillStepType)(data.GetID / 100000);
            ISkillStep step = _generateFuncByProcessType[type]?.Invoke();
            step.Refresh(data, context);

            return step;
        }
    }
}
