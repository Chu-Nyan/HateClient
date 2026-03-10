using Chu.Utility;
using System;
using System.Collections.Generic;

namespace SAB.Unit.Combat
{
    public class SkillGenerator : Singleton<SkillGenerator>
    {
        private readonly IDNumbering _numbering;
        private readonly Dictionary<SkillID, SkillData> _skillDataByID;
        private readonly Dictionary<int, IStepData> _stepDataByID;
        private Dictionary<SkillStepType, Func<ISkillStep>> _generateFuncByProcessType;

        public SkillGenerator() : base()
        {
            _skillDataByID = AssetManager.DeserializeJsonSync<Dictionary<SkillID, SkillData>>(Const.Asset_Data_SkillData);
            _stepDataByID = new();
            _numbering = new();

            InitGenerateFunc();
            InitStepData<InstanceStepData>(Const.Asset_Data_SkillStepInstance);
            InitStepData<DotStepData>(Const.Asset_Data_SkillStepDoT);
            InitStepData<AoEStepData>(Const.Asset_Data_SkillStepAoE);
            InitStepData<TimerStepData>(Const.Asset_Data_SkillStepTimer);
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

        private void InitStepData<T>(string path) where T : IStepData
        {
            Dictionary<int, T> dic = AssetManager.DeserializeJsonSync<Dictionary<int, T>>(path);

            foreach (var item in dic)
            {
                _stepDataByID.Add(item.Key, item.Value);
            }
        }

        public Skill GetSkill(SkillID id)
        {
            var skill = new Skill(_numbering.GetID());
            skill.Setup(_skillDataByID[id]);
            return skill;
        }

        public SkillSequence GenerateSequence(AttackContext context)
        {
            var data = context.SkillData;
            var list = new List<ISkillStep>();
            for (int i = 0; i < data.HitFlowStepIDs.Length; i++)
            {
                list.Add(GenerateStep(data.HitFlowStepIDs[i], context));
            }

            var sequence = new SkillSequence();
            sequence.Refresh(context, list);

            return sequence;
        }

        private ISkillStep GenerateStep(int id, AttackContext context)
        {
            var type = (SkillStepType)(id / 100000);
            ISkillStep step = _generateFuncByProcessType[type]?.Invoke();
            step.Refresh(_stepDataByID[id], context);

            return step;
        }
    }
}
