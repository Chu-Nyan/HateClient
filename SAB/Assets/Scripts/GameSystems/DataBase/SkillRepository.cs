using Chu.Collision;
using Chu.Core;
using SAB.Skill;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class SkillRepository
    {
        public const string SkillDataPath = "Dto_Skill_Base";

        public const string FlowStepPath = "Dto_Skill_FlowStep";
        public const string InstanceStepPath = "Dto_Skill_Step_Instance";
        public const string TimerStepPath = "Dto_Skill_Step_Timer";
        public const string DoTStepPath = "Dto_Skill_Step_DoT";
        public const string AoEStepPath = "Dto_Skill_Step_AoE";

        public const string HitBoxes = "Dto_Skill_HitBox";
        public const string CollisionLogic = "Dto_Skill_CollisionLogic";

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
                dtos: DataBase.ConvertJsonToArray<SkillStepInstanceDto>(AssetManager.LoadJson(InstanceStepPath)),
                keySelector: dto => dto.ID,
                converter: dto => new InstanceStepData(dto.ID, dto.DamageRate, dto.Count)
            );

            AoEStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepAoEDto>(AssetManager.LoadJson(AoEStepPath)),
                keySelector: dto => dto.ID,
                converter: dto => new AoEStepData(dto.ID, dto.DamageRate, dto.Ranged, dto.Count)
            );

            DotStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepDoTDto>(AssetManager.LoadJson(DoTStepPath)),
                keySelector: dto => dto.ID,
                converter: dto => new DotStepData(dto.ID, dto.DamageRate, dto.Duration)
            );

            TimerStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepTimerDto>(AssetManager.LoadJson(TimerStepPath)),
                keySelector: dto => dto.ID,
                converter: dto => new TimerStepData(dto.ID, dto.Duration)
            );

            FlowStepByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillFlowStepDto>(AssetManager.LoadJson(FlowStepPath)),
                keySelector: dto => dto.ID,
                converter: dto => GetStepData(dto.LogicID)
            );

            HitBoxByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillHitBoxDto>(AssetManager.LoadJson(HitBoxes)),
                keySelector: dto => dto.ID,
                converter: dto => new ShapeParam(dto.ShapeType, dto.OffsetX, dto.OffsetY, dto.Param1, dto.Param2)
            );

            CollisionLogicByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillCollisionLogicDto>(AssetManager.LoadJson(CollisionLogic)),
                keySelector: dto => dto.ID,
                converter: dto => new CollisionLogicData(dto.ID, dto.Order, dto.ActiveTime, dto.Speed, HitBoxByID[dto.HitboxID], FlowStepByID[dto.FlowStepID])
            );

            SkillByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillBaseDto>(AssetManager.LoadJson(SkillDataPath)),
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
