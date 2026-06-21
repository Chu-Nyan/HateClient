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

    // LayerGroup
    public const int Layer_Unit = (int)NyanLayer.PlayerUnit | (int)NyanLayer.NPCUnit;
    public const int Layer_AdditionalPlayerUnit = (int)NyanLayer.TriggerZone;
}
