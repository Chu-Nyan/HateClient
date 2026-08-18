using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        public string CreateScirpt(DataTableConfig config, Dictionary<string, string> namespaceByType, string userCode)
        {
            var usedNamespace = new HashSet<string>();
            var nameRow = Table.Rows[config.DBNameRow];
            var typeRow = Table.Rows[config.DBTypeRow];
            var sb = new StringBuilder();
            var namespaceText = new StringBuilder();

            sb.Append($"public class {config.GetScriptFileName(GetPascalCaseName())}\n");
            sb.Append("{\n");
            for (int i = 0; i < Table.Columns.Count; i++)
            {
                if (GoogleSheetsLoader.HasIgnoreSymbol(nameRow[i].ToString()) == true)
                    continue;

                if (namespaceByType.TryGetValue(typeRow[i].ToString(), out string ns) == true)
                    usedNamespace.Add(ns);
                sb.Append($"    public {typeRow[i]} {nameRow[i]};\n");
            }

            sb.Append("\n");
            if (userCode == null)
            {
                sb.Append($"    {DataTableConfig.UserCodeMakerHeader}\n");
                sb.Append($"    {DataTableConfig.UserCodeMakerTail}\n");
            }
            else
                sb.Append(userCode + "\n");

            sb.Append("}\n");

            foreach (var ns in usedNamespace.OrderBy(n => n))
                namespaceText.Append($"using {ns};\n");
            if (usedNamespace.Count > 0)
                namespaceText.Append("\n");

            return namespaceText.Append(sb).ToString();
        }

        public string GetPascalCaseName()
        {
            string cleanName = Table.TableName
                .Replace("_", "")
                .Replace("-", "")
                .Replace(" ", "");

            return cleanName;
        }
    }
}
