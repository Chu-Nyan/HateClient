using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
