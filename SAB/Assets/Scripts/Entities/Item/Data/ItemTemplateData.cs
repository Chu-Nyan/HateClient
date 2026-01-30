namespace SAB.Item
{
    public class ItemTemplateData
    {
        public readonly int ID;
        public readonly TextID TextID;
        public readonly ItemCategory Category;

        public ItemTemplateData(int iD, TextID textID, ItemCategory category)
        {
            ID = iD;
            TextID = textID;
            Category = category;
        }
    }
}
