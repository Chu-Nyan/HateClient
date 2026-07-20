using System;

namespace Chu.Data
{
    [Serializable]
    public class VariantParam<T> : IVariantParam
    {
        public T Value;

        public VariantParam() { }

        public VariantParam(T value)
        {
            Value = value;
        }

        public object GetValue()
        {
            return Value;
        }
    }
}
