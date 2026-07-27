using Chu.Core;
using SAB.Item;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class ItemRepository
    {
        private const string _basePath = "Dto_Item_Base";
        private const string _equipmentPath = "Dto_Item_Equipment";
        private const string _weaponPath = "Dto_Item_Weapon";

        public readonly Dictionary<int, ItemBaseData> Base;
        public readonly Dictionary<int, EquipmentBaseData> Equipment;
        public readonly Dictionary<int, WeaponBaseData> Weapon;

        public ItemRepository()
        {
            Base = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemBaseDto>(AssetManager.LoadJson(_basePath)),
                keySelector: a => a.ID,
                converter: a => new ItemBaseData(a.ID, a.NameKey, a.Category)
                );
            Equipment = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemEquipmentDto>(AssetManager.LoadJson(_equipmentPath)),
                keySelector: a => a.ID,
                converter: a => new EquipmentBaseData(a.ID, a.Slot, a.MeshPath)
                );
            Weapon = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemWeaponDto>(AssetManager.LoadJson(_weaponPath)),
                keySelector: a => a.ID,
                converter: a => new WeaponBaseData(a.ID, a.Damage, a.UpgradeSlot)
                );
        }
    }
}
