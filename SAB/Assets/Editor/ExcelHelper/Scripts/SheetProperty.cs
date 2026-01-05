using System;

[Flags]
public enum SheetProperty
{
    None = 0,
    Data = 1 << 0,
    Enum = 1 << 1,
    Localization = 1 << 2,
    ExternalFolder = 1 << 3,
}
