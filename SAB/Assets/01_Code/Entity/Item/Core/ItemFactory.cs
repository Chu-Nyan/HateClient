using Chu.Utility;
using SAB.DataManger;
using SAB.Item;
using System.Collections.Generic;

public class ItemFactory : Singleton<ItemFactory>
{
    private readonly Dictionary<int, ItemBaseData> _itemData;
    private readonly Dictionary<int, EquipmentBaseData> _equipmentData;
    private readonly Dictionary<int, WeaponBaseData> _weaponData;

    public ItemFactory(DataBase db)
    {
        _itemData = db.ItemRepo.Base;
        _equipmentData = db.ItemRepo.Equipment;
        _weaponData = db.ItemRepo.Weapon;
    }

    public IHasItemData GenerateItem(int id)
    {
        switch (_itemData[id].Category)
        {
            case ItemCategory.Weapon:
                return GenerateNewWeapon(id);
            case ItemCategory.Armor:
                return GenerateNewArmor(id);
            default:
                throw new System.Exception("지원되지 않는 아이템 카테고리 입력" + "ID : " + id);
        }
    }

    private IHasItemData GenerateNewWeapon(int id)
    {
        ItemBaseData itemData = _itemData[id];
        EquipmentBaseData equipmentData = _equipmentData[id];
        WeaponBaseData weaponData = _weaponData[id];

        var weapon = new Weapon();
        weapon.Setup(itemData, equipmentData, weaponData);
        return weapon;
    }

    private IHasItemData GenerateNewArmor(int id)
    {
        ItemBaseData itemData = _itemData[id];
        EquipmentBaseData equipmentData = _equipmentData[id];

        var armor = new Armor();
        armor.Setup(itemData, equipmentData);
        return armor;
    }
}
