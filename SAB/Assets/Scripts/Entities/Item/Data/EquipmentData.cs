namespace SAB.Item
{
    public class EquipmentData
    {
        private EquipmentTemplateData _equipmentTemplateData;
        private EquipmentInstanceData _equipmentInstanceData;

        public EquipmentData()
        {
            _equipmentInstanceData = new();
        }

        public void Setup(EquipmentTemplateData equipmentTemplateData)
        {
            _equipmentTemplateData = equipmentTemplateData;
        }
    }
}
