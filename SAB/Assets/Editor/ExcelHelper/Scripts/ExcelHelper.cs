using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "ExcelHelper", menuName = "Scriptable Objects/ExcelHelper", order = 1)]
public class ExcelHelper : ScriptableObject
{
    private const string _googleDownloadURL = "https://docs.google.com/spreadsheets/d/{0}/export?format=xlsx";
    public const char IgnoreSymbol = '#';

    public string GoogleSheetID;

    public bool IsClassAvoidDuplication = true;

    private ExcelConfigLoader _configLoader = new();
    private Dictionary<string, SheetData> _sheetDatas;
    public ConvertSetting ConvertSetting;
    public ExcelGeneratePath GeneratePath;
    private DateTime _excelUpdateTime;

    public bool HasExcelData
    {
        get => _sheetDatas != null;
    }

    public DateTime ExcelUpdateTime
    {
        get => _excelUpdateTime;
    }

    #region Excel to File 파이프라인
    public async void SetupAllInOneAsync()
    {
        await LoadExcelFile();
        await GenerateEnumScript();
        await GenerateClass();
        await GenerateDBJson();
        ConvertLocalization();
    }

    public async Task LoadExcelFile()
    {
        Debug.Log("데이터 요청 중");
        var www = UnityWebRequest.Get(string.Format(_googleDownloadURL, GoogleSheetID));
        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.LogError("실패: " + www.error);
        else
        {
            _excelUpdateTime = DateTime.Now;
            var stream = new MemoryStream(www.downloadHandler.data);
            var tables = ExcelReaderFactory.CreateReader(stream).AsDataSet().Tables;

            _sheetDatas = _configLoader.LoadSheetProperty(tables, ConvertSetting, GeneratePath);
            Debug.Log("요청 수락됨");
        }
    }
    public async Task GenerateEnumScript()
    {
        Debug.Log("Enum 스크립트 생성 시작");

        var sb = new StringBuilder();
        foreach (var item in _sheetDatas)
        {
            var sheet = item.Value;

            if (sheet.Type != SheetType.Enum)
                continue;

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
            GenerateFile(sheet.GeneratePath, $"{sheet.GetNameFromOptions()}.cs", normalizedText, true);
            await Task.Yield();
        }

        Debug.Log("Enum 스크립트 생성 완료");
    }

    public async Task GenerateClass()
    {
        if (HasExcelData == false)
            throw new Exception("엑셀 데이터 없음");

        Debug.Log("스크립트 생성 시작");
        var log = "스크립트 생성 결과\n";
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var item in _sheetDatas)
        {
            var sheet = item.Value;
            var table = sheet.Table;

            if (sheet.Type != SheetType.Data)
                continue;

            var type = assemblies
                .Select(a => a.GetType(table.TableName))
                .FirstOrDefault(t => t != null);

            if (type == null || IsClassAvoidDuplication == false)
            {
                var text = GetClassScriptText(sheet);
                GenerateFile(sheet.GeneratePath, $"{sheet.GetNameFromOptions()}.cs", text, true);

                if (IsClassAvoidDuplication == true)
                    log += $"- {table.TableName} 생성 완료\n";
                else
                    log += $"- {table.TableName} 중복 생성\n";
            }
            else
            {
                log += $"- {table.TableName} 중복 감지\n";
            }

            await Task.Yield();
        }

        Debug.Log(log);
    }

    private string GetClassScriptText(SheetData sheet)
    {
        // 주석용 열 제거
        var table = sheet.Table;
        var exceptionColumns = new HashSet<int>();
        var rows = table.Rows;
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (HasIgnoreSymbol(rows[ConvertSetting.DBNameRow][i].ToString()) == false)
                continue;

            exceptionColumns.Add(i);
        }

        // 스크립트 작성
        var code = $"public class {sheet.GetNameFromOptions()}\n";
        code += $"{{\n";
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (exceptionColumns.Contains(i) == true)
                continue;

            code += $"\t public {rows[ConvertSetting.DBTypeRow][i]} {rows[ConvertSetting.DBNameRow][i]};\n";
        }
        code += $"}}\n";

        return code;
    }

    public async Task GenerateDBJson()
    {
        if (HasExcelData == false)
            throw new Exception("DB 없음");

        Debug.Log("Json 생성 시작");
        var log = "Json 생성 결과\n";

        foreach (var item in _sheetDatas)
        {
            if (item.Value.Type != SheetType.Data)
                continue;

            var sheet = item.Value;
            var table = sheet.Table;
            var name = sheet.GetNameFromOptions();
            if (TryConvertExcelToJson(sheet, out var text) == true)
                GenerateFile(sheet.GeneratePath, $"{name}.json", text, true);

            log += text != default ? $"- {name} 생성 완료\n" : $"- {name} 생성 실패\n";

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

        var datas = new System.Object[table.Rows.Count - ConvertSetting.DBDataStartedRow];
        var filedNames = table.Rows[ConvertSetting.DBNameRow];
        var isSucceed = true;
        for (int i = 0; i < datas.Length; i++)
        {
            var data = table.Rows[ConvertSetting.DBDataStartedRow + i];
            try
            {
                var instance = Activator.CreateInstance(type);
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (HasIgnoreSymbol(table.Rows[ConvertSetting.DBNameRow][j].ToString()) == true)
                        continue;

                    var fieldInfo = type.GetField(filedNames[j].ToString());
                    if (fieldInfo.FieldType.IsEnum == true)
                    {
                        Enum.TryParse(fieldInfo.FieldType, data[j].ToString(), out var result);
                        fieldInfo.SetValue(instance, result);
                    }
                    else
                    {
                        var value = Convert.ChangeType(data[j].ToString(), fieldInfo.FieldType);
                        fieldInfo.SetValue(instance, value);
                    }
                }

                datas[i] = instance;
            }
            catch (Exception err)
            {
                Debug.Log($"{table.TableName} 클래스가 생성되지 않음\n\n {err.Message}");
                isSucceed = false;
                continue;
            }
        }
        text = isSucceed == true ? JsonConvert.SerializeObject(datas, Formatting.Indented) : "";
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

    // 현지화 텍스트 불러오기
    public void ConvertLocalization()
    {
        foreach (var sheet in _sheetDatas.Values)
        {
            if (sheet.Type != SheetType.Localization)
                continue;

            var dic = ConvertLocalizationSheetToJson(sheet.Table);
            foreach (var item in dic)
            {
                GenerateFile(sheet.GeneratePath, $"{item.Key}.json", item.Value, true);
            }
        }
    }

    private Dictionary<string, string> ConvertLocalizationSheetToJson(DataTable table)
    {
        var texts = new Dictionary<string, string>();

        for (int x = 0; x < table.Columns.Count; x++)
        {
            if (HasIgnoreSymbol(table.Rows[0][x].ToString()) == true || x == ConvertSetting.LocalizationKeyColumn)
                continue;

            var dic = new Dictionary<string, string>();

            for (int y = ConvertSetting.LocalizationFirstDataRow; y < table.Rows.Count; y++)
            {
                var key = table.Rows[y][ConvertSetting.LocalizationKeyColumn].ToString();
                if (key == string.Empty)
                    break;

                if (dic.ContainsKey(key) == true)
                    throw new Exception($"{key}중복 키 발견");

                dic.Add(key, table.Rows[y][x].ToString());
            }

            var json = JsonConvert.SerializeObject(dic, Formatting.Indented);
            var languageName = table.Rows[ConvertSetting.LocalizationNameRow][x].ToString();
            texts.Add(languageName, json);
        }

        return texts;
    }
    #endregion

    #region 유틸리티
    private void GenerateFile(string path, string fileName, string text, bool isOverwrite)
    {
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);

        var pathAndFile = Path.Combine(path, fileName);
        if (isOverwrite == true)
            File.WriteAllText(pathAndFile, text); // 덮어쓰기
        else
            File.AppendAllText(pathAndFile, text); // 이어쓰기
    }

    public static bool HasIgnoreSymbol(string text)
    {
        return text.Length == 0 || text[0] == IgnoreSymbol;
    }
    #endregion

    #region 디버그
    private void PrintExcelData(System.Data.DataTable table)
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
