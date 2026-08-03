namespace SAB.Skill
{
    public class InstanceStepData : IStepData
    {
        public readonly int ID;
        public readonly float DamageRate;
        public readonly int Count;

        public int GetID
        {
            get => ID;
        }

        public InstanceStepData(int id, float damageRate, int count)
        {
            ID = id;
            DamageRate = damageRate;
            Count = count;
        }
    }
}
