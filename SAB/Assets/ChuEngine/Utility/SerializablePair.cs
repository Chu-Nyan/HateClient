using System;

namespace Chu.Utility
{
    [Serializable]
    public class SerializablePair<T, K>
    {
        public T Key; 
        public K Value;
    }
}
