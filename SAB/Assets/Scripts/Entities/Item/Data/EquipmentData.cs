namespace SAB.Item
{
    public class EquipmentData
    {
        private EquipmentTemplateData _equipmentTemplateData;
        private EquipmentInstanceData _equipmentInstanceData;

        public EquipmentTemplateData Template
        {
            get => _equipmentTemplateData;
        }

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
