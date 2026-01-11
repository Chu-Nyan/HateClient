namespace SAB.Unit.Combat
{
    public interface ISkillStep
    {
        public bool IsDone { get; }

        public void Refresh(IStepData data, AttackContext context);
        public void Tick(IHasStats stats);
    }
}
