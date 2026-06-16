namespace Chu.Collision
{
    public struct NyanLayerMask
    {
        public int Mask;

        public NyanLayerMask(int mask)
        {
            Mask = mask;
        }

        public NyanLayerMask(NyanLayer layer)
        {
            Mask = (int)layer;
        }

        public NyanLayerMask(NyanLayer layer1, NyanLayer layer2)
        {
            Mask = (int)layer1 | (int)layer2;
        }

        public NyanLayerMask(NyanLayer layer1, NyanLayer layer2, NyanLayer layer3)
        {
            Mask = (int)layer1 | (int)layer2 | (int)layer3;
        }

        public bool ContainsLayer(NyanLayer layer)
        {
            int value = (int)layer;
            return value == (Mask & value);
        }

        public bool ContainsLayer(int layer)
        {
            return layer == (Mask & layer);
        }
    }
}
