namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬 공용 정보
    /// </summary>
    public class SkillData
    {
        public SkillID ID;
        public TextID StringID;
        public float CastingTime;
        public float Cooldown;
        public float Cost;
        public int[] FlowStepIDs;
    }
}
