using Chu.Core;
using SAB.Item;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class ItemRepository
    {
        private const string BasePath = "ItemBaseData";
        private const string EquipmentPath = "ItemEquipmentData";
        private const string WeaponPath = "ItemWeaponData";

        public readonly Dictionary<int, ItemBaseData> Base;
        public readonly Dictionary<int, EquipmentBaseData> Equipment;
        public readonly Dictionary<int, WeaponBaseData> Weapon;

        public ItemRepository()
        {
            Base = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemBaseDto>(AssetManager.LoadJson(BasePath)),
                keySelector: a => a.ID,
                converter: a => new ItemBaseData(a.ID, a.NameKey, a.Category)
                );
            Equipment = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemEquipmentDto>(AssetManager.LoadJson(EquipmentPath)),
                keySelector: a => a.ID,
                converter: a => new EquipmentBaseData(a.ID, a.Slot, a.MeshPath)
                );
            Weapon = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemWeaponDto>(AssetManager.LoadJson(WeaponPath)),
                keySelector: a => a.ID,
                converter: a => new WeaponBaseData(a.ID, a.Damage, a.UpgradeSlot)
                );
        }
    }
}
