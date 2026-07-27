using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chu.Tools
{
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
        [SerializeField] private DefaultAsset _dtoScriptPath;
        [SerializeField] private DefaultAsset _dtoJsonPath;
        [SerializeField] private DefaultAsset _enumPath;
        [SerializeField] private string _jsonPrefix;
        [SerializeField] private string _scriptSuffix;

        private const int ZeroBase = -1;

        public int DBNameRow => _dbNameRow + ZeroBase;
        public int DBTypeRow => _dbTypeRow + ZeroBase;
        public int DBDataStartedRow => _dbDataStartRow + ZeroBase;

        public int EnumDataStartedRow => _enumDataStartedRow + ZeroBase;
        public int EnumTypeColumn => _enumTypeColumn + ZeroBase;
        public int EnumKeyColumn => _enumKeyColumn + ZeroBase;
        public int EnumValueColumn => _enumValueColumn + ZeroBase;
        public int EnumCommentsColumn => _enumCommentsColumn + ZeroBase;

        public string ConfigSheetName => _configSheetName;
        public int SheetPropertyNameColumn => _sheetHeader + ZeroBase;
        public int SheetTypeColumn => _sheetTypeColumn + ZeroBase;
        public int SheetPropertyFirstDataRow => _sheetDataStartRow + ZeroBase;

        public string DTOPath
        {
            get => AssetDatabase.GetAssetPath(_dtoScriptPath);
        }

        public string DTOJsonPath
        {
            get => AssetDatabase.GetAssetPath(_dtoJsonPath);
        }

        public string EnumPath
        {
            get => AssetDatabase.GetAssetPath(_enumPath);
        }

        public string JsonPrefix
        {
            get => _jsonPrefix;
        }

        public string ScriptSuffix
        {
            get => _scriptSuffix;
        }
    }
}
