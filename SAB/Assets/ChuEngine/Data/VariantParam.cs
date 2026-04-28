using System;

namespace Chu.Data
{
    [Serializable]
    public class VariantParam<T> : IVariantParam
    {
        public T value;

        public object GetValue()
        {
            return value;
        }
    }
}
