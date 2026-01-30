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
    Weapon, Armor, Food, Material, Misc
}
