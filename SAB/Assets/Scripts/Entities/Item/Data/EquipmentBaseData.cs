namespace SAB.Item
{
    public class EquipmentBaseData
    {
        public readonly int ID;
        public readonly EquipSlot Slot;
        public readonly string MeshPath;

        public EquipmentBaseData(int id, EquipSlot slot, string meshPath)
        {
            ID = id;
            Slot = slot;
            MeshPath = meshPath;
        }
    }
}
