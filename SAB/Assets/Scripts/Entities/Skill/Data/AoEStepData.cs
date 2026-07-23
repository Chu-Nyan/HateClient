namespace SAB.Skill
{
    public class AoEStepData : IStepData
    {
        public readonly int ID;
        public readonly float DamageRate;
        public readonly float Ranged;
        public readonly int Count;

        public int GetID
        {
            get => ID;
        }

        public AoEStepData(int id, float damageRate, float ranged, int count)
        {
            ID = id;
            DamageRate = damageRate;
            Ranged = ranged;
            Count = count;
        }
    }
}
