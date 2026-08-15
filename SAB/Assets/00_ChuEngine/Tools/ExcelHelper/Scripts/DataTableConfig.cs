using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chu.Tools
{
    [Serializable]
    public class DataTableConfig
    {
        public const string UserCodeMakerHeader = "// <USER_CODE>";
        public const string UserCodeMakerTail = "// </USER_CODE>";

        public readonly Dictionary<string, string> NameSpaceByType = new()
        {
            { "ShapeType", "Chu.Collision" }
        };

        [Header("Data Sheet Config")]
        [SerializeField] private int _nameRow = 1;
        [SerializeField] private int _typeRow = 2;
        [SerializeField] private int _dataStartRow = 4;

        public DefaultAsset DefaultDtoScriptPath;
        public DefaultAsset DefaultDtoJsonPath;
        public string ScriptSuffix = "Dto";
        public string JsonSuffix = "Data";

        public int DBNameRow => _nameRow - 1;
        public int DBTypeRow => _typeRow - 1;
        public int DBDataStartedRow => _dataStartRow - 1;

        public string GetScriptFileName(string name)
        {
            return $"{name}{ScriptSuffix}";
        }

        public string GetJsonFileName(string name)
        {
            return $"{name}{JsonSuffix}";
        }
    }
}
