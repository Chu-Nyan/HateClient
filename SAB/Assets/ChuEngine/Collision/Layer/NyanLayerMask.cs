namespace Chu.Collision.Layer
{
    public struct NyanLayerMask
    {
        public int Mask;

        public NyanLayerMask(int mask)
        {
            Mask = mask;
        }

        public NyanLayerMask AddMask(NyanLayer layer)
        {
            Mask += (int)layer;
            return this;
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
