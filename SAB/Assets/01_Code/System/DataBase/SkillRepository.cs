using Chu.Collision;
using Chu.Core;
using SAB.Skill;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class SkillRepository
    {
        private const string BasePath = "Dto_Skill_Base";

        private const string FlowPath = "Dto_Skill_FlowStep";
        private const string HitBoxesPath = "Dto_Skill_HitBox";
        private const string CollisionLogicPath = "Dto_Skill_CollisionLogic";

        private const string InstancePath = "Dto_Skill_Step_Instance";
        private const string AoePath = "Dto_Skill_Step_AoE";
        private const string DotPath = "Dto_Skill_Step_DoT";
        private const string TimerPath = "Dto_Skill_Step_Timer";

        public readonly Dictionary<int, SkillData> SkillByID;

        public readonly Dictionary<int, IStepData[]> FlowStepByID;
        public readonly Dictionary<int, ShapeParam[]> HitBoxByID;
        public readonly Dictionary<int, CollisionLogicData[]> CollisionLogicByID;

        public readonly Dictionary<int, InstanceStepData> InstanceStepByID;
        public readonly Dictionary<int, AoEStepData> AoEStepByID;
        public readonly Dictionary<int, DotStepData> DotStepByID;
        public readonly Dictionary<int, TimerStepData> TimerStepByID;

        public SkillRepository()
        {
            InstanceStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepInstanceDto>(AssetManager.LoadJson(InstancePath)),
                keySelector: dto => dto.ID,
                converter: dto => new InstanceStepData(dto.ID, dto.DamageRate, dto.Count)
            );

            AoEStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepAoEDto>(AssetManager.LoadJson(AoePath)),
                keySelector: dto => dto.ID,
                converter: dto => new AoEStepData(dto.ID, dto.DamageRate, dto.Ranged, dto.Count)
            );

            DotStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepDoTDto>(AssetManager.LoadJson(DotPath)),
                keySelector: dto => dto.ID,
                converter: dto => new DotStepData(dto.ID, dto.DamageRate, dto.Duration)
            );

            TimerStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepTimerDto>(AssetManager.LoadJson(TimerPath)),
                keySelector: dto => dto.ID,
                converter: dto => new TimerStepData(dto.ID, dto.Duration)
            );

            FlowStepByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillFlowStepDto>(AssetManager.LoadJson(FlowPath)),
                keySelector: dto => dto.ID,
                converter: dto => GetStepData(dto.LogicID)
            );

            HitBoxByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillHitBoxDto>(AssetManager.LoadJson(HitBoxesPath)),
                keySelector: dto => dto.ID,
                converter: dto => new ShapeParam(dto.ShapeType, dto.OffsetX, dto.OffsetY, dto.Param1, dto.Param2)
            );

            CollisionLogicByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillCollisionLogicDto>(AssetManager.LoadJson(CollisionLogicPath)),
                keySelector: dto => dto.ID,
                converter: dto => new CollisionLogicData(dto.ID, dto.Order, dto.ActiveTime, dto.Speed, HitBoxByID[dto.HitboxID], FlowStepByID[dto.FlowStepID])
            );

            SkillByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillBaseDto>(AssetManager.LoadJson(BasePath)),
                keySelector: dto => dto.ID,
                converter: dto => new SkillData(dto, CollisionLogicByID[dto.ObjectLogicID])
            );
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
    }
}
