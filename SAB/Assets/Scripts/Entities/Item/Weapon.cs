namespace SAB.Item
{
    public class Weapon : IHasItemData, IHasEquipmentData
    {
        private ItemData _itemData;
        private EquipmentData _equipmentData;
        private WeaponTemplateData _weaponData;

        public ItemData ItemData
        {
            get => _itemData;
        }

        public EquipmentData EquipmentTemplateData
        {
            get => _equipmentData;
        }
    }
}
