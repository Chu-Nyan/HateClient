namespace SAB.Unit
{
    public class BaseStats
    {
        public readonly int ID;
        public readonly FactionType Faction;
        public readonly TextID NameID;
        public readonly TextID DescID;
        public readonly float[] Stats;

        public BaseStats(int iD, FactionType faction, TextID nameID, TextID descID, float[] stats)
        {
            ID = iD;
            Faction = faction;
            NameID = nameID;
            DescID = descID;
            Stats = stats;
        }
    }
}
