using Chu.Collision;
using SAB.Unit.Combat;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class SkillRepository
    {
        public const string SkillDataPath = "SkillData";

        public const string FlowStepPath = "SkillFlowStep";
        public const string InstanceStepPath = "SkillStepInstance";
        public const string TimerStepPath = "SkillStepTimer";
        public const string DoTStepPath = "SkillStepDoT";
        public const string AoEStepPath = "SkillStepAoE";

        public const string HitBoxes = "HitBoxData";
        public const string CollisionLogic = "CollisionLogic";

        public readonly Dictionary<SkillID, SkillData> SkillByID;

        public readonly Dictionary<int, IStepData[]> FlowStepByID;
        public readonly Dictionary<int, ShapeParam[]> HitBoxByID;

        public readonly Dictionary<int, InstanceStepData> InstanceStepByID;
        public readonly Dictionary<int, AoEStepData> AoEStepByID;
        public readonly Dictionary<int, DotStepData> DotStepByID;
        public readonly Dictionary<int, TimerStepData> TimerStepByID;

        public SkillRepository()
        {
            InstanceStepByID = DeserializeInstanceStep();
            AoEStepByID = DeserializeAoEStep();
            DotStepByID = DeserializeDotStep();
            TimerStepByID = DeserializeTimerStep();
            FlowStepByID = DeserializeFlowStepData();

            HitBoxByID = DeserializeHitBox();
            SkillByID = DeserializeSkillData();
        }

        private Dictionary<SkillID, SkillData> DeserializeSkillData()
        {
            var dic = new Dictionary<SkillID, SkillData>();
            var dto = AssetManager.DeserializeJsonSync<Dictionary<SkillID, Skill_Base_DTO>>(SkillDataPath);

            foreach (var item in dto)
            {
                // TODO : 히트박스 개편시 변경해야함
                var dtoData = dto[item.Key];
                dic[item.Key] = new SkillData(dtoData, HitBoxByID[(int)dtoData.ID - 99999], FlowStepByID[dtoData.FlowStepID]);
            }

            return dic;
        }

        private Dictionary<int, ShapeParam[]> DeserializeHitBox()
        {
            var dic = new Dictionary<int, ShapeParam[]>();
            var dtoDic = AssetManager.DeserializeJsonSync<Dictionary<int, Skill_HitBox_DTO[]>>(HitBoxes);

            foreach (var item in dtoDic)
            {
                var dto = item.Value;
                dic[item.Key] = new ShapeParam[dto.Length];
                for (int i = 0; i < dto.Length; i++)
                {
                    dic[item.Key][i] = new ShapeParam(dto[i].ShapeType, dto[i].OffsetX, dto[i].OffsetY, dto[i].Param1, dto[i].Param2);
                }
            }

            return dic;
        }

        private Dictionary<int, IStepData[]> DeserializeFlowStepData()
        {
            var flowByID = new Dictionary<int, IStepData[]>();
            var flow = AssetManager.DeserializeJsonSync<Dictionary<int, Skill_FlowStep_DTO[]>>(FlowStepPath);

            foreach (var item in flow)
            {
                var dtoArr = item.Value;
                var id = item.Key;

                if (flowByID.ContainsKey(id) == false)
                    flowByID[id] = new IStepData[dtoArr.Length];

                for (int i = 0; i < dtoArr.Length; i++)
                {
                    flowByID[id][i] = GetStepData(dtoArr[i].LogicID);
                }
            }

            return flowByID;
        }

        private IStepData GetStepData(int id)
        {
            if (InstanceStepByID.ContainsKey(id) == true)
                return InstanceStepByID[id];
            if (AoEStepByID.ContainsKey(id) == true)
                return AoEStepByID[id];
            if (DotStepByID.ContainsKey(id) == true)
                return DotStepByID[id];
            if (TimerStepByID.ContainsKey(id) == true)
                return TimerStepByID[id];

            throw new Exception($"Don't have skill step {id}");
        }


        private Dictionary<int, InstanceStepData> DeserializeInstanceStep()
        {
            var dic = new Dictionary<int, InstanceStepData>();
            var dto = AssetManager.DeserializeJsonSync<Dictionary<int, Skill_Step_Instance_DTO>>(InstanceStepPath);

            foreach (var item in dto)
            {
                var dtoData = item.Value;
                var data = new InstanceStepData(dtoData.ID, dtoData.DamageRate, dtoData.Count);
                dic.Add(item.Value.ID, data);
            }

            return dic;
        }

        private Dictionary<int, AoEStepData> DeserializeAoEStep()
        {
            var dic = new Dictionary<int, AoEStepData>();
            var dto = AssetManager.DeserializeJsonSync<Dictionary<int, Skill_Step_AoE_DTO>>(AoEStepPath);

            foreach (var item in dto)
            {
                var dtoData = item.Value;
                var data = new AoEStepData(dtoData.ID, dtoData.DamageRate, dtoData.Ranged, dtoData.Count);
                dic.Add(item.Value.ID, data);
            }

            return dic;
        }

        private Dictionary<int, DotStepData> DeserializeDotStep()
        {
            var dic = new Dictionary<int, DotStepData>();
            var dto = AssetManager.DeserializeJsonSync<Dictionary<int, Skill_Step_DoT_DTO>>(DoTStepPath);

            foreach (var item in dto)
            {
                var dtoData = item.Value;
                var data = new DotStepData(dtoData.ID, dtoData.DamageRate, dtoData.Duration);
                dic.Add(item.Value.ID, data);
            }

            return dic;
        }

        private Dictionary<int, TimerStepData> DeserializeTimerStep()
        {
            var dic = new Dictionary<int, TimerStepData>();
            var dto = AssetManager.DeserializeJsonSync<Dictionary<int, Skill_Step_Timer_DTO>>(TimerStepPath);

            foreach (var item in dto)
            {
                var dtoData = item.Value;
                var data = new TimerStepData(dtoData.ID, dtoData.Duration);
                dic.Add(item.Value.ID, data);
            }

            return dic;
        }
    }
}
