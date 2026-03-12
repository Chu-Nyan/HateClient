namespace SAB.Unit
{
    public class BaseStats
    {
        public readonly int ID;
        public readonly TextID NameID;
        public readonly TextID DescID;
        public readonly float[] Stats;

        public BaseStats(int iD, TextID nameID, TextID descID, float[] stats)
        {
            ID = iD;
            NameID = nameID;
            DescID = descID;
            Stats = stats;
        }
    }
}
