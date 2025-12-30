namespace Chu.Collision.Layer
{
    public readonly struct NyanLayerMask
    {
        public readonly int Mask;

        public NyanLayerMask(int mask)
        {
            Mask = mask;
        }

        public bool HasFlag(int layer)
        {
            return layer == (Mask & layer);
        }
    }
}
