namespace SAB.Item
{
    public class Weapon : IHasItemData, IHasEquipmentData
    {
        private ItemData _itemData;
        private EquipmentData _equipmentData;
        private WeaponBaseData _weaponData;

        public ItemData ItemData
        {
            get => _itemData;
        }

        public EquipmentData EquipmentTemplateData
        {
            get => _equipmentData;
        }

        public Weapon()
        {
            _itemData = new();
            _equipmentData = new();
        }

        public void Setup(ItemBaseData itemData, EquipmentBaseData equipmentData, WeaponBaseData weaponData)
        {
            _itemData.Setup(itemData);
            _equipmentData.Setup(equipmentData);
            _weaponData = weaponData;
        }
    }
}
