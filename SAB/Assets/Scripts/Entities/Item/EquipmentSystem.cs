namespace SAB.Item
{
    public class EquipmentSystem
    {
        private IHasEquipmentData[] _parts;

        public EquipmentSystem()
        {
            _parts = new IHasEquipmentData[5];
        }

        public void Equip(IHasEquipmentData item)
        {
            _parts[(int)item.EquipmentTemplateData.Template.Slot] = item;
        }
    }
}
