using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ExcelHelper", menuName = "Scriptable Objects/ExcelHelper", order = 1)]
public class ExcelHelper : ScriptableObject
{
    public GoogleSheetsLoader ExcelLoader;
    public DBConvertConfig ConvertSetting;

    private Dictionary<SheetType, List<SheetData>> _sheetsByType;

    public bool HasExcelData
    {
        get => _sheetsByType != null;
    }

    #region Excel to File 파이프라인
    public async void SetupAllInOneAsync()
    {
        await LoadExcelFile();
        await GenerateEnumScript();
        await GenerateClass();
        await ExportDataToJson();
    }

    public async Task LoadExcelFile()
    {
        await ExcelLoader.RequestExcelFile();
        _sheetsByType = ParseSheet(ExcelLoader.Sheets, ConvertSetting);
    }

    private Dictionary<SheetType, List<SheetData>> ParseSheet(DataTableCollection table, DBConvertConfig setting)
    {
        var sheetDatas = new Dictionary<SheetType, List<SheetData>>();
        var config = table[ConvertSetting.ConfigSheetName];
        var duplicateChecker = new HashSet<string>();

        for (int x = setting.SheetPropertyFirstDataRow; x < config.Rows.Count; x++)
        {
            var sheet = config.Rows[x];
            var name = sheet[setting.SheetPropertyNameColumn].ToString();
            if (table.Contains(name) == false)
            {
                Debug.LogError($"{name} : 존재 하지 않는 시트 이름");
                continue;
            }
            if (duplicateChecker.Contains(name) == true)
            {
                Debug.LogError($"{name} : 시트 중복");
                continue;
            }
            if (Enum.TryParse(sheet[setting.SheetTypeColumn].ToString(), out SheetType type) == false)
            {
                Debug.LogError($"{name} : 잘못된 SheetType");
                continue;
            }

            if (sheetDatas.TryGetValue(type, out var list) == false)
                sheetDatas[type] = list = new();

            list.Add(new SheetData(table[name], type, ConvertSetting.GetPath(type)));
        }
        Debug.Log("시트 불러오기 완료");
        return sheetDatas;
    }

    private async Task GenerateEnumScript()
    {
        var sb = new StringBuilder();
        foreach (var sheet in _sheetsByType[SheetType.Enum])
        {
            var list = GetEnumScriptText(sheet);
            sb.Clear();
            for (int j = 0; j < list.Count; j++)
            {
                if (j + 1 < list.Count)
                    sb.AppendLine(list[j]);
                else
                    sb.Append(list[j]);
            }

            var normalizedText = sb.ToString().Replace("\r\n", "\n").Replace("\n", "\r\n");
            ExcelUtility.GenerateFile(sheet.GeneratePath, $"{sheet.GetNameFromOptions()}.cs", normalizedText);
            await Task.Yield();
        }

        Debug.Log("Enum 스크립트 생성 완료");
    }

    private async Task GenerateClass()
    {
        if (HasExcelData == false)
            throw new Exception("엑셀 데이터 없음");

        var log = "스크립트 생성 결과\n";
        foreach (SheetData sheet in _sheetsByType[SheetType.Data])
        {
            var text = GetDataScriptText(sheet, ConvertSetting.NameSpaceByType);
            ExcelUtility.GenerateFile(sheet.GeneratePath, $"{sheet.GetNameFromOptions()}.cs", text);
            log += $"- {sheet.Table.TableName} 생성\n";

            await Task.Yield();
        }

        Debug.Log(log);
    }

    private string GetDataScriptText(SheetData sheet, Dictionary<string, string> namespaceByType)
    {
        var usedNamespace = new HashSet<string>();
        var nameRow = sheet.Table.Rows[ConvertSetting.DBNameRow];
        var typeRow = sheet.Table.Rows[ConvertSetting.DBTypeRow];
        var sb = new StringBuilder();
        var namespaceText = new StringBuilder();

        sb.AppendLine($"public struct {sheet.GetNameFromOptions()}");
        sb.AppendLine("{");
        for (int i = 0; i < sheet.Table.Columns.Count; i++)
        {
            if (ExcelUtility.HasIgnoreSymbol(nameRow[i].ToString()) == true)
                continue;

            if (namespaceByType.TryGetValue(typeRow[i].ToString(), out string ns) == true)
                usedNamespace.Add(ns);
            sb.AppendLine($"\t public {typeRow[i]} {nameRow[i]};");
        }
        sb.AppendLine("}");

        foreach (var ns in usedNamespace.OrderBy(n => n))
            namespaceText.AppendLine($"using {ns};");
        if (usedNamespace.Count > 0)
            namespaceText.AppendLine();

        return namespaceText.Append(sb).ToString();
    }

    private async Task ExportDataToJson()
    {
        if (HasExcelData == false)
            throw new Exception("DB 없음");

        var log = "Json 생성 결과\n";

        foreach (SheetData sheet in _sheetsByType[SheetType.Data])
        {
            var name = sheet.GetNameFromOptions();
            if (TryConvertExcelToJson(sheet, out var text) == true)
                ExcelUtility.GenerateFile(sheet.GeneratePath, $"{name}.json", text);

            log += text != default ? $"- {name} 생성 완료\n" : $"- {name} 오류 발생\n";

            await Task.Yield();
        }

        Debug.Log(log);
    }

    private bool TryConvertExcelToJson(SheetData sheet, out string text)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var table = sheet.Table;
        Type type = assemblies
            .Select(a => a.GetType(sheet.GetNameFromOptions()))
            .FirstOrDefault(t => t != null);

        var fieldMap = type.GetFields().ToDictionary(f => f.Name);
        var fieldNames = table.Rows[ConvertSetting.DBNameRow];
        var datas = new object[table.Rows.Count - ConvertSetting.DBDataStartedRow];
        var isSucceed = true;

        for (int i = 0; i < datas.Length; i++)
        {
            var data = table.Rows[ConvertSetting.DBDataStartedRow + i];

            var instance = Activator.CreateInstance(type);
            for (int j = 0; j < table.Columns.Count; j++)
            {
                string fieldName = fieldNames[j].ToString();

                if (ExcelUtility.HasIgnoreSymbol(table.Rows[ConvertSetting.DBNameRow][j].ToString()) == true)
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
                    Debug.LogError($"{table.TableName}, {ConvertSetting.DBDataStartedRow + i + 1}행 {fieldName} {data[j]} 변환 실패");
                    isSucceed = false;
                }
            }

            datas[i] = instance;
        }
        text = JsonConvert.SerializeObject(datas, Formatting.Indented);
        return isSucceed;
    }

    private List<string> GetEnumScriptText(SheetData sheet)
    {
        var rows = sheet.Table.Rows;
        var sb = new StringBuilder();
        var arr = new List<string>(8);
        var template = "public enum {0}\n{{\n{1}}}\n";

        var index = ConvertSetting.EnumDataStartedRow;

        while (index < rows.Count)
        {
            var typeText = rows[index][ConvertSetting.EnumTypeColumn].ToString();

            sb.Clear();
            while (index < rows.Count && rows[index][ConvertSetting.EnumTypeColumn].ToString() == typeText)
            {
                sb.Append($"\t{rows[index][ConvertSetting.EnumKeyColumn]} = {rows[index][ConvertSetting.EnumValueColumn]},");
                if (rows[index][ConvertSetting.EnumCommentsColumn].ToString() != string.Empty)
                {
                    sb.Append($" // {rows[index][ConvertSetting.EnumCommentsColumn]}");
                }
                sb.AppendLine();
                index++;
            }
            arr.Add(String.Format(template, typeText, sb.ToString()));
        }

        return arr;
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
