using Chu.Utility;
using System.Collections.Generic;

namespace SAB.Unit.Combat
{
    public class SkillGenerator : Singleton<SkillGenerator>
    {
        private Dictionary<SkillID, SkillData> _skillDataByID;
        private Dictionary<int, IStepData> _stepDataByID;

        public SkillGenerator() : base()
        {
            _skillDataByID = AssetManager.DeserializeJsonSync<Dictionary<SkillID, SkillData>>(Const.Asset_Data_SkillData);
            _stepDataByID = new();
            InitStepData<InstanceStepData>(Const.Asset_Data_SkillFlowInstance);
            InitStepData<DotStepData>(Const.Asset_Data_SkillFlowDoT);
            InitStepData<AoEStepData>(Const.Asset_Data_SkillFlowAoE);
            InitStepData<TimerStepData>(Const.Asset_Data_SkillFlowTimer);
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
            var skill = new Skill();
            skill.Init(_skillDataByID[id]);
            return skill;
        }
    }
}
