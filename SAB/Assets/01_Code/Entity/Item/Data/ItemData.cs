namespace SAB.Item
{
    public class ItemData
    {
        private ItemBaseData _itemTemplateData;
        private ItemInstanceData _itemInstanceData;

        public ItemData()
        {
            _itemInstanceData = new ItemInstanceData();
        }

        public void Setup(ItemBaseData itemTemplateData)
        {
            _itemTemplateData = itemTemplateData;
        }
    }
}
