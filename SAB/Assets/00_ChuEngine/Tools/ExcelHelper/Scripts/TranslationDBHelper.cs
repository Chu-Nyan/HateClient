using Chu.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;

namespace Chu.Tools
{
    [CreateAssetMenu(fileName = "TranslationDBHelper", menuName = "Scriptable Objects/TranslationDBHelper", order = 1)]
    public class TranslationDBHelper : ScriptableObject
    {
        public TranslationDBConfig Config;
        public GoogleSheetsLoader ExcelLoader;

        public void ExportDbToJson()
        {
            var path = AssetDatabase.GetAssetPath(Config.GeneratePath);
            for (int i = 0; i < ExcelLoader.Sheets.Count; i++)
            {
                var dic = ConvertSheetToJson(ExcelLoader.Sheets[i]);
                foreach (var item in dic)
                {
                    FileUtility.GenerateFile(path, $"{item.Key}.json", item.Value);
                }
            }

            Debug.Log($"Translation JSON files generated successfully.");
        }

        private Dictionary<string, string> ConvertSheetToJson(DataTable table)
        {
            var texts = new Dictionary<string, string>();
            var dic = new Dictionary<string, string>();

            for (int x = 0; x < table.Columns.Count; x++)
            {
                if (GoogleSheetsLoader.HasIgnoreSymbol(table.Rows[0][x].ToString()) == true || x == Config.KeyColumn)
                    continue;

                dic.Clear();

                for (int y = Config.FirstDataRow; y < table.Rows.Count; y++)
                {
                    var key = table.Rows[y][Config.KeyColumn].ToString();
                    if (key == string.Empty)
                        throw new Exception($"{y}행의 키값이 비어있음");
                    if (dic.ContainsKey(key) == true)
                        throw new Exception($"{key} : 중복 키 발견");

                    dic.Add(key, table.Rows[y][x].ToString());
                }

                var json = JsonConvert.SerializeObject(dic, Formatting.Indented);
                var languageName = table.Rows[Config.HeaderRow][x].ToString();
                texts.Add(languageName, json);
            }

            return texts;
        }
    }
}
