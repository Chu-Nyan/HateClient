namespace SAB.Unit.Combat
{
    public class AoEStepData : IStepData
    {
        public int ID;
        public float DamageRate;
        public float Ranged;
        public int Count;

        public int GetID
        {
            get => ID;
        }
    }
}
