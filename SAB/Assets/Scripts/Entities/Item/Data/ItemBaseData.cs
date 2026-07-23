namespace SAB.Item
{
    public class ItemBaseData
    {
        public readonly int ID;
        public readonly TextID TextID;
        public readonly ItemCategory Category;

        public ItemBaseData(int id, TextID textID, ItemCategory category)
        {
            ID = id;
            TextID = textID;
            Category = category;
        }
    }
}
