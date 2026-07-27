namespace SAB.Unit
{
    public class BaseStats
    {
        public readonly int ID;
        public readonly FactionType Faction;
        public readonly string NameKey;
        public readonly string DescKey;
        public readonly float[] Stats;

        public BaseStats(int iD, FactionType faction, string nameKey, string descKey, float[] stats)
        {
            ID = iD;
            Faction = faction;
            NameKey = nameKey;
            DescKey = descKey;
            Stats = stats;
        }
    }
}
