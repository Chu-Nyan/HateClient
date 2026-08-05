using Chu.Collision;
using Chu.Core;
using SAB.Skill;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class SkillRepository
    {
        private const string BasePath = "SkillBaseData";

        private const string SequencePath = "SkillSequenceData";
        private const string HitBoxesPath = "SkillHitBoxData";
        private const string CollisionLogicPath = "SkillCollisionLogicData";

        private const string InstancePath = "SkillStepInstanceData";
        private const string AoePath = "SkillStepAoEData";
        private const string DotPath = "SkillStepDoTData";
        private const string TimerPath = "SkillStepTimerData";

        public readonly Dictionary<int, SkillData> SkillByID;

        public readonly Dictionary<int, IStepData[]> SequenceByID;
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

            SequenceByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillFlowStepDto>(AssetManager.LoadJson(SequencePath)),
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
                converter: dto => new CollisionLogicData(dto.ID, dto.Order, dto.ActiveTime, dto.Speed, HitBoxByID[dto.HitboxID], SequenceByID[dto.SequenceID])
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
