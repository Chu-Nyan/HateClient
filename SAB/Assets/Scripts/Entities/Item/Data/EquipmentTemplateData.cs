namespace SAB.Item
{
    public class EquipmentTemplateData
    {
        public readonly int ID;
        public readonly EquipSlot Slot;
        public readonly string MeshPath;

        public EquipmentTemplateData(int id, EquipSlot slot, string meshPath)
        {
            ID = id;
            Slot = slot;
            MeshPath = meshPath;
        }
    }
}
