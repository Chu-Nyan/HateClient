namespace SAB.Unit.Combat
{
    public class InstanceStepData : IStepData
    {
        public int ID;
        public float DamageRate;
        public int Count;

        public int GetID
        {
            get => ID;
        }
    }
}
