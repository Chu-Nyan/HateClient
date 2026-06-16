using Chu.Collision;
using SAB.Unit.Combat;

/// <summary>
/// 공격 시 제공하는 순수한 데미지 정보
/// </summary>
public struct AttackContext
{
    public SkillData SkillData;
    public NyanLayerMask Mask;

    public float Damage;

    public AttackContext(SkillData data, NyanLayerMask mask, float damage)
    {
        SkillData = data;

        Mask = mask;
        Damage = damage;
    }
}
