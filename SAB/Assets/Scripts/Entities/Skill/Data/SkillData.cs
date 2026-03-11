using Chu.Collision;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬 공용 정보
    /// </summary>
    public class SkillData
    {
        public readonly SkillID ID;
        public readonly TextID StringID;
        public readonly float CastingTime;
        public readonly float Cooldown;
        public readonly float Cost;

        public readonly ShapeParam[] HitBoxes;
        public readonly IStepData[] OnHitFlowStep;

        public SkillData(Skill_Base_DTO dto, ShapeParam[] hitBoxes, IStepData[] flowStep)
        {
            ID = dto.ID;
            StringID = dto.StringID;
            CastingTime = dto.CastingTime;
            Cooldown = dto.Cooldown;
            Cost = dto.Cost;

            HitBoxes = hitBoxes;
            OnHitFlowStep = flowStep;
        }
    }
}
