using Chu.Utility;
using SAB.Item;
using System.Collections.Generic;

public class ItemFactory : Singleton<ItemFactory>
{
    private Dictionary<int, ItemTemplateData> _itemData;
    private Dictionary<int, EquipmentTemplateData> _equipmentData;
    private Dictionary<int, WeaponTemplateData> _weaponData;

    public ItemFactory()
    {
        _itemData = AssetManager.DeserializeJsonSync<Dictionary<int, ItemTemplateData>>("ItemData");
        _equipmentData = AssetManager.DeserializeJsonSync<Dictionary<int, EquipmentTemplateData>>("EquipmentData");
        _weaponData = AssetManager.DeserializeJsonSync<Dictionary<int, WeaponTemplateData>>("WeaponData");
    }

    public IHasItemData GenerateItem(int id)
    {
        switch (_itemData[id].Category)
        {
            case ItemCategory.Weapon:
                return GenerateNewWeapon(id);
            case ItemCategory.Armor:
                return GenerateNewArmor(id);
            case ItemCategory.Food:
            case ItemCategory.Material:
            case ItemCategory.Misc:
            default:
                throw new System.Exception("지원되지 않는 아이템 카테고리 입력" + "ID : " + id);
        }
    }

    private IHasItemData GenerateNewWeapon(int id)
    {
        ItemTemplateData itemData = _itemData[id];
        EquipmentTemplateData equipmentData = _equipmentData[id];
        WeaponTemplateData weaponData = _weaponData[id];

        var weapon = new Weapon();
        weapon.Setup(itemData, equipmentData, weaponData);
        return weapon;
    }

    private IHasItemData GenerateNewArmor(int id)
    {
        ItemTemplateData itemData = _itemData[id];
        EquipmentTemplateData equipmentData = _equipmentData[id];

        var armor = new Armor();
        armor.Setup(itemData, equipmentData);
        return armor;
    }
}
