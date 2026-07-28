using SAB.Skill;

namespace SAB.GameSystem
{
    /// <summary>
    /// 공격 정보를 제공할 수 있는 객체
    /// </summary>
    public interface IAttackContextProvider
    {
        public AttackContext Context { get; }
    }
}
