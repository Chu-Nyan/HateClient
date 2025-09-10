using System.Collections.Generic;

public class CharacterBaseStats
{
    public const int MaxLevel = 10;
    public UnitType Type;
    public string Name;
    public string Desc;
    public float Speed;
    public Dictionary<int, LevelStats> LevelData;
    public DropItemTable DropTable;
}
