namespace SAB.Cutscene
{
    public readonly struct CutsceneObjectConfig
    {
        public readonly int ID;
        public readonly CutsceneObjectType Type;
        public readonly IObjectConfig ObjectConfig;

        public CutsceneObjectConfig(int id, CutsceneObjectType type, IObjectConfig config)
        {
            ID = id;
            Type = type;
            ObjectConfig = config;
        }
    }
}
