using Chu.Collision;

public static class Const
{
    public const string Asset_Character = "Character";

    public const string Asset_Mesh_CharacterBody = "CharacterBody";

    // Cutscene
    public const string Asset_CutsceneManger = "CutSceneManager";
    public const string Asset_VCamStatic = "VCamStatic";
    public const string Asset_VCamFollow = "VCamFollow";
    public const string Asset_SingleMesh = "SingleMesh";

    // DB
    public const string Asset_DB_MapGeneratedData = "{0}GeneratedData";
    public const string Asset_DB_CutsceneData = "{0}CutsceneData";


    // Tag
    public const string Tag_MapReferenceHub = "MapReferenceHub";

    // File Path
    public const string Path_DB_Map = "DB/Map";
    public const string Path_DB_Cutscene = "DB/Cutscene";

    // LayerGroup
    public const int Layer_Unit = (int)NyanLayer.PlayerUnit | (int)NyanLayer.NPCUnit;
    public const int Layer_AdditionalPlayerUnit = (int)NyanLayer.TriggerZone;
}
