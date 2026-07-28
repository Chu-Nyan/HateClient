using SAB.Skill;

/// <summary>
/// 공격을 받을 수 있는 객체
/// </summary>
namespace SAB.GameSystem
{
    public interface IDefendable
    {
        public FactionType FactionType { get; }
        public void Defend(AttackContext context, HitResult hit);
    }
}
