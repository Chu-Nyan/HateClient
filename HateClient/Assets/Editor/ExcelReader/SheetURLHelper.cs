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
    private static readonly HashSet<string> _uniqueSheetName = new()
    {
        "Enum"
    };

    public string GoogleSheetID;
    public int DataNameRow = 0;
    public int DataTypeRow = 1;
    public int DataStartedRow = 2;

    public int EnumDataStartedRow = 2;
    public int EnumTypeColumn = 0;
    public int EnumKeyColumn = 1;
    public int EnumValueColumn = 2;
    public int EnumCommentsColumn = 3;

    public bool IsClassAvoidDuplication = true;
    public string ClassGeneratedPath = Application.dataPath;
    public string DBGeneratedPath = Application.dataPath;
    public string EnumGeneratedPath = Application.dataPath;

    private DataSet _excelData;
    private DateTime _excelUpdateTime;

    public bool HasExcelData
    {
        get => _excelData != null;
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

        if (www.result == UnityWebRequest.Result.Success)
        {
            var stream = new MemoryStream(www.downloadHandler.data);
            _excelData = ExcelReaderFactory.CreateReader(stream)
                                           .AsDataSet();
            _excelUpdateTime = DateTime.Now;
            Debug.Log("요청 수락됨");
        }
        else
        {
            Debug.LogError("실패: " + www.error);
        }
    }

    public void GenerateClass()
    {
        if (HasExcelData == false)
            throw new Exception("엑셀 데이터 없음");

        Debug.Log("스크립트 생성 시작");
        var log = "스크립트 생성 결과\n";
        var tables = _excelData.Tables;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < tables.Count; i++)
        {
            if (_uniqueSheetName.Contains(tables[i].TableName) == true)
                continue;

            var type = assemblies
                .Select(a => a.GetType(tables[i].TableName))
                .FirstOrDefault(t => t != null);

            if (type == null || IsClassAvoidDuplication == false)
            {
                var text = GetClassScriptText(tables[i]);
                GenerateFile(ClassGeneratedPath, $"{tables[i].TableName}.cs", text, true);
                if (IsClassAvoidDuplication == true)
                {
                    log += $"- {tables[i].TableName} 생성 완료\n";
                }
                else
                {
                    log += $"- {tables[i].TableName} 중복 생성\n";
                }
            }
            else
            {
                log += $"- {tables[i].TableName} 중복 감지\n";
            }
        }

        Debug.Log(log);
    }

    private string GetClassScriptText(DataTable table)
    {
        // 주석용 열 제거
        var exceptionColumns = new HashSet<int>();
        var sheet = table.Rows;
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (IsPassRow(table, i) == true)
            {
                exceptionColumns.Add(i);
            }
        }

        // 스크립트 작성
        var code = $"public class {table.TableName}\n";
        code += $"{{\n";
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (exceptionColumns.Contains(i) == true)
                continue;

            code += $"\t public {sheet[DataTypeRow][i]} {sheet[DataNameRow][i]};\n";
        }
        code += $"}}\n";

        return code;
    }

    public void GenerateDBJson()
    {
        if (HasExcelData == false)
            throw new Exception("DB 없음");

        Debug.Log("Json 생성 시작");
        var log = "Json 생성 결과\n";
        for (int i = 0; i < _excelData.Tables.Count; i++)
        {
            if (_uniqueSheetName.Contains(_excelData.Tables[i].TableName) == true)
                continue;

            var table = _excelData.Tables[i];
            if (TryConvertExcelToJson(table, out var text) == true)
            {
                GenerateFile(DBGeneratedPath, $"{table.TableName}.json", text, true);
            }
            log += text != default ? $"- {table.TableName} 생성 완료\n" : $"- {table.TableName} 생성 실패\n";
        }
        Debug.Log(log);
    }

    private bool TryConvertExcelToJson(DataTable table, out string text)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type type = assemblies
            .Select(a => a.GetType(table.TableName))
            .FirstOrDefault(t => t != null);

        var datas = new System.Object[table.Rows.Count - DataStartedRow];
        var filedNames = table.Rows[DataNameRow];
        var isSucceed = true;
        for (int i = 0; i < datas.Length; i++)
        {
            var data = table.Rows[DataStartedRow + i];
            try
            {
                var instance = Activator.CreateInstance(type);
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (IsPassRow(table, j) == true)
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

    public void GenerateEnumScript()
    {
        Debug.Log("Enum 스크립트 생성 시작");
        for (int i = 0; i < _excelData.Tables.Count; i++)
        {
            if (_excelData.Tables[i].TableName != "Enum")
                continue;

            var list = GetEnumScriptText(_excelData.Tables[i].Rows);
            var sb = new StringBuilder();

            // TODO : 스크립트를 하나로 통일 or x줄 이상 분리 등등 분기 만들기
            for (int j = 0; j < list.Count; j++)
            {
                if (j + 1 < list.Count)
                    sb.AppendLine(list[j]);
                else
                    sb.Append(list[j]);
            }

            var normalizedText = sb.ToString().Replace("\r\n", "\n").Replace("\n", "\r\n");
            GenerateFile(EnumGeneratedPath, "Enum.cs", normalizedText, true);
        }
        Debug.Log("Enum 스크립트 생성 완료");
    }

    public async void SetupAllInOneAsync()
    {
        await LoadExcelFile();
        GenerateEnumScript();
        GenerateClass();
        GenerateDBJson();
    }

    private List<string> GetEnumScriptText(DataRowCollection rows)
    {
        var sb = new StringBuilder();
        var arr = new List<string>(8);
        var template = "public enum {0}\n{{\n{1}}}\n";

        for (int i = EnumDataStartedRow - 1; i < rows.Count; i++)
        {
            var typeText = rows[i][EnumTypeColumn].ToString();

            sb.Clear();
            while (i < rows.Count && rows[i][EnumTypeColumn].ToString() == typeText)
            {
                // TODO : 매 키 마다 밸류는 적을 것이냐, 분기에만 적을 것이냐
                sb.Append($"\t{rows[i][EnumKeyColumn]} = {rows[i][EnumValueColumn]},");
                if (rows[i][EnumCommentsColumn].ToString() != string.Empty)
                {
                    sb.Append($" // {rows[i][EnumCommentsColumn]}");
                }
                sb.AppendLine();
                i++;
            }
            arr.Add(String.Format(template, typeText, sb.ToString()));
        }

        return arr;
    }

    private bool IsPassRow(DataTable table, int index)
    {
        return (table.Rows[DataNameRow][index].ToString().Length > 0 && table.Rows[DataNameRow][index].ToString()[0] == '#');
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
}