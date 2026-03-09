using System;
using UnityEngine;

[Serializable]
public class ConvertSetting
{
    [SerializeField] private int _dbNameRow = 1;
    [SerializeField] private int _dbTypeRow = 2;
    [SerializeField] private int _dbDataStartedRow = 4;

    [SerializeField] private int _enumDataStartedRow = 2;
    [SerializeField] private int _enumTypeColumn = 1;
    [SerializeField] private int _enumKeyColumn = 2;
    [SerializeField] private int _enumValueColumn = 3;
    [SerializeField] private int _enumCommentsColumn = 4;

    [SerializeField] private int _localizationNameRow = 1;
    [SerializeField] private int _localizationKeyColumn = 1;
    [SerializeField] private int _localizationFirstDataRow = 2;

    [SerializeField] private int _sheetPropertyNameColumn = 1;
    [SerializeField] private int _sheetPropertyTypeColumn = 2;
    [SerializeField] private int _sheetPropertyFirstDataRow = 3;

    public bool IsZeroBase = true;

    public int DBNameRow => GetBaseValue(_dbNameRow);
    public int DBTypeRow => GetBaseValue(_dbTypeRow);
    public int DBDataStartedRow => GetBaseValue(_dbDataStartedRow);

    public int EnumDataStartedRow => GetBaseValue(_enumDataStartedRow);
    public int EnumTypeColumn => GetBaseValue(_enumTypeColumn);
    public int EnumKeyColumn => GetBaseValue(_enumKeyColumn);
    public int EnumValueColumn => GetBaseValue(_enumValueColumn);
    public int EnumCommentsColumn => GetBaseValue(_enumCommentsColumn);

    public int LocalizationNameRow => GetBaseValue(_localizationNameRow);
    public int LocalizationKeyColumn => GetBaseValue(_localizationKeyColumn);
    public int LocalizationFirstDataRow => GetBaseValue(_localizationFirstDataRow);

    public int SheetPropertyNameColumn => GetBaseValue(_sheetPropertyNameColumn);
    public int SheetPropertyTypeColumn => GetBaseValue(_sheetPropertyTypeColumn);
    public int SheetPropertyFirstDataRow => GetBaseValue(_sheetPropertyFirstDataRow);

    private int GetBaseValue(int value)
    {
        return IsZeroBase == true ? value - 1 : value;
    }
}
