using Chu.Utility;

namespace SAB.DataManger
{
    public class DataBase : Singleton<DataBase>
    {
        public readonly SkillRepository SkillRepo;

        public DataBase()
        {
            SkillRepo = new SkillRepository();
        }
    }
}
