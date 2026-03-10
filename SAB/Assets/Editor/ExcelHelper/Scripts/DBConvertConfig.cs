using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class DBConvertConfig
{
    public readonly Dictionary<string, string> NameSpaceByType = new()
    {
        { "ShapeType", "Chu.Collision" }
    }; 

    [Header("Data Sheet Config")]
    [SerializeField] private int _dbNameRow = 1;
    [SerializeField] private int _dbTypeRow = 2;
    [SerializeField] private int _dbDataStartRow = 4;
    [Space, Header("Enum Sheet Config")]
    [SerializeField] private int _enumDataStartedRow = 2;
    [SerializeField] private int _enumTypeColumn = 1;
    [SerializeField] private int _enumKeyColumn = 2;
    [SerializeField] private int _enumValueColumn = 3;
    [SerializeField] private int _enumCommentsColumn = 4;
    [Space, Header("Individual Sheet Config")]
    [SerializeField] private string _configSheetName = "@SheetConfig";
    [SerializeField] private int _sheetHeader = 1;
    [SerializeField] private int _sheetTypeColumn = 2;
    [SerializeField] private int _sheetDataStartRow = 3;
    [Space, Header("Export Path")]
    [SerializeField] private DefaultAsset _dtoPath;
    [SerializeField] private DefaultAsset _enumPath;

    private const int _zeroBase = -1;

    public int DBNameRow => _dbNameRow + _zeroBase;
    public int DBTypeRow => _dbTypeRow + _zeroBase;
    public int DBDataStartedRow => _dbDataStartRow + _zeroBase;

    public int EnumDataStartedRow => _enumDataStartedRow + _zeroBase;
    public int EnumTypeColumn => _enumTypeColumn + _zeroBase;
    public int EnumKeyColumn => _enumKeyColumn + _zeroBase;
    public int EnumValueColumn => _enumValueColumn + _zeroBase;
    public int EnumCommentsColumn => _enumCommentsColumn + _zeroBase;

    public string ConfigSheetName => _configSheetName;
    public int SheetPropertyNameColumn => _sheetHeader + _zeroBase;
    public int SheetTypeColumn => _sheetTypeColumn + _zeroBase;
    public int SheetPropertyFirstDataRow => _sheetDataStartRow + _zeroBase;

    public string GetPath(SheetType type)
    {
        if (type == SheetType.Data)
            return AssetDatabase.GetAssetPath(_dtoPath);
        else /*(type == SheetType.Enum)*/
            return AssetDatabase.GetAssetPath(_enumPath);
    }
}
