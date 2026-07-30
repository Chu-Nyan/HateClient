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
        public string ConfigSheetName = "@SheetConfig";
        [SerializeField] private int _sheetHeader = 1;
        [SerializeField] private int _sheetTypeColumn = 2;
        [SerializeField] private int _sheetDataStartRow = 3;

        [Space, Header("Export Path")]
        [SerializeField] private DefaultAsset _dtoScriptPath;
        [SerializeField] private DefaultAsset _dtoJsonPath;
        [SerializeField] private DefaultAsset _enumPath;
        public string JsonPrefix;
        public string ScriptSuffix;

        public int DBNameRow => _dbNameRow - 1;
        public int DBTypeRow => _dbTypeRow - 1;
        public int DBDataStartedRow => _dbDataStartRow - 1;

        public int EnumDataStartedRow => _enumDataStartedRow - 1;
        public int EnumTypeColumn => _enumTypeColumn - 1;
        public int EnumKeyColumn => _enumKeyColumn - 1;
        public int EnumValueColumn => _enumValueColumn - 1;
        public int EnumCommentsColumn => _enumCommentsColumn - 1;

        public int SheetPropertyNameColumn => _sheetHeader - 1;
        public int SheetTypeColumn => _sheetTypeColumn - 1;
        public int SheetPropertyFirstDataRow => _sheetDataStartRow - 1;

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
    }
}
