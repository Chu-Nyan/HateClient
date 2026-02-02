namespace SAB.Item
{
    public class Armor : IHasItemData, IHasEquipmentData
    {
        private ItemData _itemData;
        private EquipmentData _equipmentData;

        public ItemData ItemData
        {
            get => _itemData;
        }

        public EquipmentData EquipmentTemplateData
        {
            get => _equipmentData;
        }

        public Armor()
        {
            _itemData = new();
            _equipmentData = new();
        }

        public void Setup(ItemTemplateData itemData, EquipmentTemplateData equipment)
        {
            _itemData.Setup(itemData);
            _equipmentData.Setup(equipment);
        }
    }
}
