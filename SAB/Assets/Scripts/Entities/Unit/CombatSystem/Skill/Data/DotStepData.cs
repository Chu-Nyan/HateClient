namespace SAB.Unit.Combat
{
    public class DotStepData : IStepData
    {
        public int ID;
        public float DamageRate;
        public float Duration;

        public int GetID
        {
            get => ID;
        }
    }
}
