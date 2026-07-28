using System;

namespace Chu.AI
{
    public interface IMachineStateResolver<T, K> where T : Enum
    {
        public T Resolve(K context);
    }
}
