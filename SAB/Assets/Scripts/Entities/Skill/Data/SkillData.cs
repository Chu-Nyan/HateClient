using Chu.Collision;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬 공용 정보
    /// </summary>
    public class SkillData
    {
        public readonly int ID;
        public readonly TextID StringID;
        public readonly float CastingTime;
        public readonly float Cooldown;
        public readonly float Cost;

        public readonly ShapeParam[] HitBoxes;
        public readonly IStepData[] OnHitFlowStep;
        public readonly CollisionLogicData[] CollisionLogics;

        public SkillData(SkillBaseDto dto, ShapeParam[] hitBoxes, IStepData[] flowStep, CollisionLogicData[] collisionLogics)
        {
            ID = dto.ID;
            StringID = dto.StringID;
            CastingTime = dto.CastingTime;
            Cooldown = dto.Cooldown;
            Cost = dto.Cost;

            HitBoxes = hitBoxes;
            OnHitFlowStep = flowStep;
            CollisionLogics = collisionLogics;
        }
    }
}
