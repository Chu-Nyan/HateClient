using System;
using System.Data;
using UnityEditor;

namespace Chu.Tools
{
    [Serializable]
    public class SheetData
    {
        [NonSerialized]
        public DataTable Table;
        public DefaultAsset ScriptPath;
        public DefaultAsset JsonPath;

        public string GetPacalCaseName()
        {
            string cleanName = Table.TableName
                .Replace("_", "")
                .Replace("-", "")
                .Replace(" ", "");

            return cleanName;
        }
    }
}
