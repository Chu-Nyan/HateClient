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

        public readonly Dictionary<int, ItemTemplateData> Base;
        public readonly Dictionary<int, EquipmentTemplateData> Equipment;
        public readonly Dictionary<int, WeaponTemplateData> Weapon;

        public ItemRepository()
        {
            Base = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemBaseDto>(AssetManager.LoadJson(_basePath)),
                keySelector: a => a.ID,
                converter: a => new ItemTemplateData(a.ID, a.TextID, a.Category)
                );
            Equipment = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemEquipmentDto>(AssetManager.LoadJson(_equipmentPath)),
                keySelector: a => a.ID,
                converter: a => new EquipmentTemplateData(a.ID, a.Slot, a.MeshPath)
                );
            Weapon = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<ItemWeaponDto>(AssetManager.LoadJson(_weaponPath)),
                keySelector: a => a.ID,
                converter: a => new WeaponTemplateData(a.ID, a.Damage, a.UpgradeSlot)
                );
        }
    }
}
