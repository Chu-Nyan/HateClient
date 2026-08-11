using System;

namespace SAB.GameSystem
{
    [Flags]
    public enum InputEventType
    {
        None = 0,
        Performed = 1 << 0,
        Canceled = 1 << 1
    }
}
