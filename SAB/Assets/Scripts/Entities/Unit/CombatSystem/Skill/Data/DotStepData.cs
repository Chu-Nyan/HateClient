namespace SAB.Unit.Combat
{
    public class DotStepData : IStepData
    {
        public const float DamageInterval = 1f;
        public int ID;
        public float DamageRate;
        public float Duration;

        public int GetID
        {
            get => ID;
        }
    }
}
