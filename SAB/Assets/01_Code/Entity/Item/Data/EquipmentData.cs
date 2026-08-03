namespace SAB.Item
{
    public class EquipmentData
    {
        private EquipmentBaseData _equipmentTemplateData;
        private EquipmentInstanceData _equipmentInstanceData;

        public EquipmentBaseData Template
        {
            get => _equipmentTemplateData;
        }

        public EquipmentData()
        {
            _equipmentInstanceData = new();
        }

        public void Setup(EquipmentBaseData equipmentTemplateData)
        {
            _equipmentTemplateData = equipmentTemplateData;
        }
    }
}
