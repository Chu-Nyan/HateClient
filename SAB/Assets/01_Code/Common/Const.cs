using Chu.Collision;

public static class Const
{
    // DB
    public const string Asset_DB_MapGenerated = "{0}GeneratedData";
    public const string Asset_DB_Cutscene = "{0}CutsceneData";

    // LayerGroup
    public const int Layer_Unit = (int)NyanLayer.PlayerUnit | (int)NyanLayer.NPCUnit;
    public const int Layer_AdditionalPlayerUnit = (int)NyanLayer.TriggerZone;
}
