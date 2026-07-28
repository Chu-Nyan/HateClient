using Chu.Collision;

namespace SAB.Skill
{
    /// <summary>
    /// 공격 시 제공하는 순수한 데미지 정보
    /// </summary>
    public struct AttackContext
    {
        public SkillData SkillData;
        public FactionType Faction;
        public NyanLayerMask Mask;

        public float Damage;

        public AttackContext(SkillData data, FactionType faction, NyanLayerMask mask, float damage)
        {
            SkillData = data;
            Faction = faction;

            Mask = mask;
            Damage = damage;
        }
    }
}
