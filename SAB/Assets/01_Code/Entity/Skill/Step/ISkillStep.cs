namespace SAB.Skill
{
    public interface ISkillStep
    {
        public bool IsDone { get; }

        public void Setup(IStepData data, AttackContext context);
        public void Tick(IHasStats stats);
    }
}
