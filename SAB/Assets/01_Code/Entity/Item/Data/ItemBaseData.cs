namespace SAB.Item
{
    public class ItemBaseData
    {
        public readonly int ID;
        public readonly string TextKey;
        public readonly ItemCategory Category;

        public ItemBaseData(int id, string textKey, ItemCategory category)
        {
            ID = id;
            TextKey = textKey;
            Category = category;
        }
    }
}
