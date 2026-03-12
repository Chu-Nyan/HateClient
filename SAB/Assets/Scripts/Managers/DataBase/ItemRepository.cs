using SAB.Item;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class ItemRepository
    {
        private const string _basePath = "ItemData";
        private const string _equipmentPath = "EquipmentData";
        private const string _weaponPath = "WeaponData";

        public readonly Dictionary<int, ItemTemplateData> Base;
        public readonly Dictionary<int, EquipmentTemplateData> Equipment;
        public readonly Dictionary<int, WeaponTemplateData> Weapon;

        public ItemRepository()
        {

            Base = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Item_Base_DTO[]>(_basePath),
                keySelector: a => a.ID,
                converter: a => new ItemTemplateData(a.ID, a.TextID, a.Category)
                );
            Equipment = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Item_Equipment_DTO[]>(_equipmentPath),
                keySelector: a => a.ID,
                converter: a => new EquipmentTemplateData(a.ID, a.Slot, a.MeshPath)
                );
            Weapon = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Item_Weapon_DTO[]>(_weaponPath),
                keySelector: a =>a.ID,
                converter: a => new WeaponTemplateData(a.ID,a.Damage,a.UpgradeSlot)
                );
        }
    }
}
