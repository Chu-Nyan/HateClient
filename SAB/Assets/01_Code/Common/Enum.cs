using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum ItemType
{
    Phone = 10000,
    Card = 20000,
    HealingPotion = 10000,
    ManaPotion = 10001,
    Elixir = 10002,
    IronSword = 20000,
    SteelShield = 20001,
}

public enum StatType
{
    HP, ATK, PDEF, MDEF, SPD
}

public enum MapType
{
    Forest,
}

public enum FactionType
{
    Player = 0,
    Kingdom,
    Underworld,
}

public enum FactionRelation
{
    Hostile = -1,
    Neutral = 0,
    Friendly = 1
}

[JsonConverter(typeof(StringEnumConverter))]
public enum EquipSlot
{
    Helmet, Body, Cloak, Shield, Weapon
}

[JsonConverter(typeof(StringEnumConverter))]
public enum ItemCategory
{
    Weapon, Armor
}

[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponType
{
    Unarmed, Sword, Spear, Bow
}

[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponHandedness
{
    OneHand,
    TwoHand
}

[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponStance
{
    Unarmed, Sword, SwordAndShield
}

public enum CustomizingPart
{
    Eye, Eyebrow, Hair, Mouth
}

[JsonConverter(typeof(StringEnumConverter))]
public enum SkillStepType
{
    Instant = 1,
    DoT = 2,
    AoE = 3,
    Timer = 4,
}

[JsonConverter(typeof(StringEnumConverter))]
public enum Gender
{
    Male = 0,
    Female = 1
}
