namespace SAB.Unit.Combat
{
    public class DotStepData : IStepData
    {
        public const float DamageInterval = 1f;

        public readonly int ID;
        public readonly float DamageRate;
        public readonly float Duration;

        public int GetID
        {
            get => ID;
        }

        public DotStepData(int id, float damageRate, float duration)
        {
            ID = id;
            DamageRate = damageRate;
            Duration = duration;
        }
    }
}
