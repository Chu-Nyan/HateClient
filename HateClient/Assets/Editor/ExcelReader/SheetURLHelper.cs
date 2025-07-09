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

public class SheetURLHelper : ScriptableObject
{
    private const string _googleDownloadURL = "https://docs.google.com/spreadsheets/d/{0}/export?format=xlsx";
    private const char _ignoreSymbol = '#';
    private const string _sheetSettingName = "SheetSetting";
    private const string _convertSettingName = "ConvertSetting";

    public string GoogleSheetID;

    public bool IsClassAvoidDuplication = true;
    public string ClassGeneratedPath = Application.dataPath;
    public string DBGeneratedPath = Application.dataPath;
    public string EnumGeneratedPath = Application.dataPath;
    public string ExternalFolderGeneratedPath = Application.dataPath;
    public string LocalizationGeneratedPath = Application.dataPath;

    private Dictionary<string, SheetData> _sheetDatas;
    private ConvertSetting _convertSetting;
    private DateTime _excelUpdateTime;

    public bool HasExcelData
    {
        get => _sheetDatas != null;
    }

    public DateTime ExcelUpdateTime
    {
        get => _excelUpdateTime;
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
            var stream = new MemoryStream(www.downloadHandler.data);
            var tables = ExcelReaderFactory.CreateReader(stream).AsDataSet().Tables;

            SetExcelData(tables);
            SetConvertSetting(tables[_convertSettingName]);
            Debug.Log("요청 수락됨");
        }
    }

    private void SetExcelData(DataTableCollection table)
    {
        _excelUpdateTime = DateTime.Now;
        _sheetDatas = new();
        var option = table[_sheetSettingName];

        for (int x = 1; x < option.Rows.Count; x++)
        {
            var flag = 0;
            for (int y = 1; y < option.Columns.Count; y++)
            {
                if (Enum.TryParse<ExcelReadConvertType>(option.Rows[x][y].ToString(), out var result) == false)
                    continue;

                flag += (int)result;
            }

            var name = option.Rows[x][0].ToString();

            if (_sheetDatas.TryAdd(option.Rows[x][0].ToString(), new(table[name], flag)) == false)
                Debug.LogError($"{name}가 이미 존재함");
        }
    }

    private void SetConvertSetting(DataTable sheet)
    {
        _convertSetting = new ConvertSetting();
        var type = _convertSetting.GetType();
        var nameColumn = 0;
        var valueColumn = 1;

        for (int i = 1; i < sheet.Rows.Count; i++)
        {
            var fieldName = sheet.Rows[i][nameColumn].ToString();
            if (HasIgnoreSymbol(fieldName) == true)
                continue;

            var fieldInfo = type.GetField(fieldName);
            if (fieldInfo != null)
            {
                var value = Convert.ChangeType(sheet.Rows[i][valueColumn].ToString(), fieldInfo.FieldType);
                fieldInfo.SetValue(_convertSetting, value);
            }
            else
            {
                Debug.Log($"{fieldInfo} 누락");
            }
        }

        _convertSetting.SetZeroBase();
    }

    public async Task GenerateEnumScript()
    {
        Debug.Log("Enum 스크립트 생성 시작");

        var sb = new StringBuilder();
        foreach (var item in _sheetDatas)
        {
            var sheet = item.Value;

            if (sheet.HasFlag(ExcelReadConvertType.Enum) == false)
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

            var path = sheet.HasFlag(ExcelReadConvertType.ExternalFolder) == false ? EnumGeneratedPath : ExternalFolderGeneratedPath;
            GenerateFile(path, $"{sheet.GetNameFromOptions()}.cs", normalizedText, true);
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

            if (sheet.HasFlag(ExcelReadConvertType.Data) == false)
                continue;

            var type = assemblies
                .Select(a => a.GetType(table.TableName))
                .FirstOrDefault(t => t != null);

            if (type == null || IsClassAvoidDuplication == false)
            {
                var text = GetClassScriptText(sheet);
                var path = sheet.HasFlag(ExcelReadConvertType.ExternalFolder) == false ? ClassGeneratedPath : ExternalFolderGeneratedPath;
                GenerateFile(path, $"{sheet.GetNameFromOptions()}.cs", text, true);

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
        var table = sheet.Table;
        // 주석용 열 제거
        var exceptionColumns = new HashSet<int>();
        var rows = table.Rows;
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (HasIgnoreSymbol(rows[_convertSetting.DBNameRow][i].ToString()) == false)
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

            code += $"\t public {rows[_convertSetting.DBTypeRow][i]} {rows[_convertSetting.DBNameRow][i]};\n";
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
            if (item.Value.HasFlag(ExcelReadConvertType.Data) == false)
                continue;

            var sheet = item.Value;
            var table = sheet.Table;
            var name = sheet.GetNameFromOptions();
            if (TryConvertExcelToJson(table, out var text) == true)
            {
                var path = sheet.HasFlag(ExcelReadConvertType.ExternalFolder) == false ? DBGeneratedPath : ExternalFolderGeneratedPath;
                GenerateFile(path, $"{name}.json", text, true);
            }

            log += text != default ? $"- {name} 생성 완료\n" : $"- {name} 생성 실패\n";

            await Task.Yield();
        }

        Debug.Log(log);
    }

    private bool TryConvertExcelToJson(DataTable table, out string text)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type type = assemblies
            .Select(a => a.GetType(table.TableName))
            .FirstOrDefault(t => t != null);

        var datas = new System.Object[table.Rows.Count - _convertSetting.DBDataStartedRow];
        var filedNames = table.Rows[_convertSetting.DBNameRow];
        var isSucceed = true;
        for (int i = 0; i < datas.Length; i++)
        {
            var data = table.Rows[_convertSetting.DBDataStartedRow + i];
            try
            {
                var instance = Activator.CreateInstance(type);
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (HasIgnoreSymbol(table.Rows[_convertSetting.DBNameRow][j].ToString()) == true)
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
            catch (ArgumentNullException err)
            {
                Debug.Log($"{table.TableName} 클래스가 생성되지 않음\n\n {err.Message}");
                isSucceed = false;
                break;
            }
        }
        text = isSucceed == true ? JsonConvert.SerializeObject(datas, Formatting.Indented) : "";
        return isSucceed;
    }

    public async void SetupAllInOneAsync()
    {
        await LoadExcelFile();
        await GenerateEnumScript();
        await GenerateClass();
        await GenerateDBJson();
        ConvertLocalization();
    }

    private List<string> GetEnumScriptText(SheetData sheet)
    {
        var rows = sheet.Table.Rows;
        var sb = new StringBuilder();
        var arr = new List<string>(8);
        var template = "public enum {0}\n{{\n{1}}}\n";

        var index = _convertSetting.EnumDataStartedRow;

        while (index < rows.Count)
        {
            var typeText = rows[index][_convertSetting.EnumTypeColumn].ToString();

            sb.Clear();
            while (index < rows.Count && rows[index][_convertSetting.EnumTypeColumn].ToString() == typeText)
            {
                // TODO : 매 키 마다 밸류는 적을 것이냐, 분기에만 적을 것이냐
                sb.Append($"\t{rows[index][_convertSetting.EnumKeyColumn]} = {rows[index][_convertSetting.EnumValueColumn]},");
                if (rows[index][_convertSetting.EnumCommentsColumn].ToString() != string.Empty)
                {
                    sb.Append($" // {rows[index][_convertSetting.EnumCommentsColumn]}");
                }
                sb.AppendLine();
                index++;
            }
            typeText = sheet.GetNameFromOptions();
            arr.Add(String.Format(template, typeText, sb.ToString()));
        }

        return arr;
    }

    // 현지화 텍스트 불러오기
    public void ConvertLocalization()
    {
        foreach (var sheet in _sheetDatas.Values)
        {
            if (sheet.HasFlag(ExcelReadConvertType.Localization) == false)
                continue;

            var dic = ConvertLocalizationSheetToJson(sheet.Table);
            foreach (var item in dic)
            {
                var path = sheet.HasFlag(ExcelReadConvertType.ExternalFolder) == false ? LocalizationGeneratedPath : ExternalFolderGeneratedPath;
                var name = sheet.GetNameFromOptions();
                GenerateFile(path, $"{name}.json", item.Value, true);
            }
        }
    }

    private Dictionary<string, string> ConvertLocalizationSheetToJson(DataTable table)
    {
        var texts = new Dictionary<string, string>();

        for (int x = 0; x < table.Columns.Count; x++)
        {
            if (HasIgnoreSymbol(table.Rows[0][x].ToString()) == true || x == _convertSetting.LocalizationKeyColumn)
                continue;

            var dic = new Dictionary<string, string>();

            for (int y = _convertSetting.LocalizationFirstDataRow; y < table.Rows.Count; y++)
            {
                var key = table.Rows[y][_convertSetting.LocalizationKeyColumn].ToString();
                if (key == string.Empty)
                    break;

                if (dic.ContainsKey(key) == true)
                    throw new Exception("중복된 키 발견");

                dic.Add(key, table.Rows[y][x].ToString());
            }

            var json = JsonConvert.SerializeObject(dic, Formatting.Indented);
            var languageName = table.Rows[_convertSetting.LocalizationNameRow][x].ToString();
            texts.Add(languageName, json);
        }

        return texts;
    }

    #region 유틸리티
    private void GenerateFileWithOption()
    {
        // TODO
    }

    private void GenerateFile(string path, string fileName, string text, bool isOverwrite)
    {
        // TODO : Path가 무조건 Asset 내부에서만
        path = Path.Combine(path, fileName);
        if (isOverwrite == true)
            File.WriteAllText(path, text); // 덮어쓰기
        else
            File.AppendAllText(path, text); // 이어쓰기
    }

    private bool HasIgnoreSymbol(string text)
    {
        return text.Length == 0 || text[0] == _ignoreSymbol;
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
