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

        public readonly Dictionary<int, SkillData> SkillByID;

        public readonly Dictionary<int, IStepData[]> FlowStepByID;
        public readonly Dictionary<int, ShapeParam[]> HitBoxByID;

        public readonly Dictionary<int, InstanceStepData> InstanceStepByID;
        public readonly Dictionary<int, AoEStepData> AoEStepByID;
        public readonly Dictionary<int, DotStepData> DotStepByID;
        public readonly Dictionary<int, TimerStepData> TimerStepByID;

        public SkillRepository()
        {
            InstanceStepByID = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Skill_Step_Instance_DTO[]>(InstanceStepPath),
                keySelector: dto => dto.ID,
                converter: dto => new InstanceStepData(dto.ID, dto.DamageRate, dto.Count)
                );

            AoEStepByID = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Skill_Step_AoE_DTO[]>(AoEStepPath),
                keySelector: dto => dto.ID,
                converter: dto => new AoEStepData(dto.ID, dto.DamageRate, dto.Ranged, dto.Count)
                );

            DotStepByID = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Skill_Step_DoT_DTO[]>(DoTStepPath),
                keySelector: dto => dto.ID,
                converter: dto => new DotStepData(dto.ID, dto.DamageRate, dto.Duration)
                );

            TimerStepByID = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Skill_Step_Timer_DTO[]>(TimerStepPath),
                keySelector: dto => dto.ID,
                converter: dto => new TimerStepData(dto.ID, dto.Duration)
                );

            FlowStepByID = DataBase.DeserializeArrayByKey(
                dtos: AssetManager.DeserializeJsonSync<Skill_FlowStep_DTO[]>(FlowStepPath),
                keySelector: dto => dto.ID,
                converter: dto => GetStepData(dto.LogicID)
                );

            HitBoxByID = DataBase.DeserializeArrayByKey(
                dtos: AssetManager.DeserializeJsonSync<Skill_HitBox_DTO[]>(HitBoxes),
                keySelector: dto => dto.ID,
                converter: dto => new ShapeParam(dto.ShapeType, dto.OffsetX, dto.OffsetY, dto.Param1, dto.Param2)
                );

            SkillByID = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Skill_Base_DTO[]>(SkillDataPath),
                keySelector: dto => dto.ID,
                converter: dto => new SkillData(dto, HitBoxByID[dto.ID], FlowStepByID[dto.FlowStepID])
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
