using Chu.Utility;

namespace SAB.Unit.Combat
{
    public interface ISkillStep
    {
        public bool IsDone { get; }

        public void Refresh(IStepData data, AttackContext dmg);
        public void Tick(IHasStats stats);
    }
}
