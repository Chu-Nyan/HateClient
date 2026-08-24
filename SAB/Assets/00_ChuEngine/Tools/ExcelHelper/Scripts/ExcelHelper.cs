using Chu.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Chu.Tools
{
    [CreateAssetMenu(fileName = "ExcelHelper", menuName = "Scriptable Objects/ExcelHelper", order = 1)]
    public class ExcelHelper : ScriptableObject
    {
        public GoogleSheetsLoader ExcelLoader;
        public DataTableConfig Config;

        [SerializeField]
        public Dictionary<string, SheetData> SheetsByName;

        #region Excel to File 파이프라인
        public async void SetupAllInOneAsync()
        {
            await LoadExcelFile();
            //await GenerateEnumScript();
            await GenerateClass();
            await ExportDataToJson();
            AssetDatabase.Refresh();

            Debug.Log("생성 완료");
        }

        public async Task LoadExcelFile()
        {
            await ExcelLoader.RequestExcelFile();
            ParseSheet(ExcelLoader.Sheets);
        }

        private void ParseSheet(DataTableCollection table)
        {
            if (SheetsByName == null)
                SheetsByName = new();

            var duplicateChecker = new HashSet<string>();
            for (int i = 0; i < table.Count; i++)
            {
                var sheet = table[i];
                var name = sheet.TableName;

                if (GoogleSheetsLoader.HasIgnoreSymbol(name) == true)
                    continue;

                if (duplicateChecker.Contains(name) == true)
                {
                    Debug.LogError($"{name} : 시트 중복");
                    continue;
                }

                if (SheetsByName.TryGetValue(name, out var data) == false)
                {
                    data = new();
                    data.ScriptPath = Config.DefaultDtoScriptPath;
                    data.JsonPath = Config.DefaultDtoJsonPath;
                    data.NameSpace = Config.DefaultDtoNameSpace;
                    SheetsByName[name] = data;
                }

                data.Setup(table[name], Config);
                duplicateChecker.Add(name);
            }

            foreach (var key in SheetsByName.Keys.ToArray())
            {
                if (duplicateChecker.Contains(key) == true)
                    continue;

                SheetsByName.Remove(key);
            }
        }

        private async Task GenerateClass()
        {
            foreach (var item in SheetsByName)
            {
                var path = AssetDatabase.GetAssetPath(item.Value.ScriptPath);
                var name = Config.GetScriptFileName(item.Value.GetPascalCaseName());
                var filePath = Path.Combine(path, $"{name}.cs");
                string userCode;

                if (File.Exists(filePath) == true)
                    userCode = DTOScriptCreater.ChangeGeneratedCode(File.ReadAllText(filePath), item.Value.FieldNameByType);
                else
                    userCode = DTOScriptCreater.CreateScript(Config, item.Value);

                FileUtility.WriteTextFileWithLf(path, $"{name}.cs", userCode);

                await Task.Yield();
            }
        }

        private async Task ExportDataToJson()
        {
            foreach (var item in SheetsByName)
            {
                var sheet = item.Value;
                var name = item.Key;
                if (TryConvertExcelToJson(sheet, out var text) == true)
                {
                    var path = AssetDatabase.GetAssetPath(sheet.JsonPath);
                    FileUtility.GenerateFile(path, $"{Config.GetJsonFileName(sheet.GetPascalCaseName())}.json", text);
                }

                if (text == default)
                {
                    Debug.LogError($"JSON conversion failed, {name}");
                }

                await Task.Yield();
            }
        }

        private bool TryConvertExcelToJson(SheetData sheet, out string text)
        {
            var assemblies = UnityEngine.Assemblies.CurrentAssemblies.GetLoadedAssemblies();
            var table = sheet.Table;
            Type type = assemblies
                .Select(a => a.GetType($"{Config.GetScriptFileName($"{sheet.NameSpace}.{sheet.GetPascalCaseName()}")}"))
                .FirstOrDefault(t => t != null);

            var fieldMap = type.GetFields().ToDictionary(f => f.Name);
            var fieldNames = table.Rows[Config.DBNameRow];
            var datas = new object[table.Rows.Count - Config.DBDataStartedRow];
            var isSucceed = true;

            for (int i = 0; i < datas.Length; i++)
            {
                var data = table.Rows[Config.DBDataStartedRow + i];

                var instance = Activator.CreateInstance(type);
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    string fieldName = fieldNames[j].ToString();

                    if (GoogleSheetsLoader.HasIgnoreSymbol(table.Rows[Config.DBNameRow][j].ToString()) == true)
                        continue;
                    if (fieldMap.TryGetValue(fieldName, out var fieldInfo) == false)
                        continue;

                    try
                    {
                        if (fieldInfo.FieldType.IsEnum == true)
                        {
                            fieldInfo.SetValue(instance, Enum.Parse(fieldInfo.FieldType, data[j].ToString()));
                        }
                        else
                        {
                            var value = Convert.ChangeType(data[j].ToString(), fieldInfo.FieldType);
                            fieldInfo.SetValue(instance, value);
                        }
                    }
                    catch
                    {
                        Debug.LogError($"{table.TableName}, {Config.DBDataStartedRow + i + 1}행 {fieldName} {data[j]} 변환 실패");
                        isSucceed = false;
                    }
                }

                datas[i] = instance;
            }
            text = JsonConvert.SerializeObject(datas, Formatting.Indented);
            return isSucceed;
        }
        #endregion

        #region 디버그
        private void PrintExcelData(DataTable table)
        {
            string rowData = "";
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    rowData += table.Rows[i][j]?.ToString() + " ";
                }
                rowData += "\n";
            }
            Debug.Log(rowData);
        }

        private void PrintExcelData(System.Data.DataTableCollection tables)
        {
            for (int i = 0; i < tables.Count; i++)
            {
                PrintExcelData(tables[i]);
            }
        }
        #endregion
    }
}
