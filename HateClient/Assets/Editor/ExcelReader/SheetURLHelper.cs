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

    private string _googleSheetID;
    private int _dataNameRowNumber;
    private int _dataTypeRowNumber;
    private int _dataStartedRowNumber;
    private int _zeroBaseNameRow;
    private int _zeroBaseTypeRow;
    private int _zeroBaseDataStartedRow;

    private int _enumDataStartedRow = 2;
    private int _enumTypeColumn = 1 - 1;
    private int _enumKeyColumn = 2 - 1;
    private int _enumValueColumn = 3 - 1;
    private int _enumCommentsColumn = 4 - 1;

    private string _classGeneratedPath;
    private bool _isClassAvoidDuplication;
    private string _dbGeneratedPath;
    private string _enumGeneratedPath;

    private DataSet _excelData;
    private DateTime _excelUpdateTime;

    public string GoogleSheetID
    {
        get => _googleSheetID;
        set => _googleSheetID = value;
    }

    public int DataNameRowNumber
    {
        get => _dataNameRowNumber;
        set
        {
            _zeroBaseNameRow = value - 1;
            _dataNameRowNumber = value;
        }
    }

    public int DataTypeRowNumber
    {
        get => _dataTypeRowNumber;
        set
        {
            _zeroBaseTypeRow = value - 1;
            _dataTypeRowNumber = value;
        }
    }

    public int DataStartedRowNumber
    {
        get => _dataStartedRowNumber;
        set
        {
            _zeroBaseDataStartedRow = value - 1;
            _dataStartedRowNumber = value;
        }
    }

    public string ClassGeneratedPath
    {
        get => _classGeneratedPath;
        set => _classGeneratedPath = value;
    }

    public string DBGeneratedPath
    {
        get => _dbGeneratedPath;
        set => _dbGeneratedPath = value;
    }

    public string EnumGeneratedPath
    {
        get => _enumGeneratedPath;
        set => _enumGeneratedPath = value;
    }

    public bool IsClassAvoidDuplication
    {
        get => _isClassAvoidDuplication;
        set => _isClassAvoidDuplication = value;
    }

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
        var www = UnityWebRequest.Get(string.Format(_googleDownloadURL, _googleSheetID));
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

            if (type == null || _isClassAvoidDuplication == false)
            {
                var text = GetClassScriptText(tables[i]);
                GenerateFile(_classGeneratedPath, $"{tables[i].TableName}.cs", text, true);
                if (_isClassAvoidDuplication == true)
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

            code += $"\t public {sheet[_zeroBaseTypeRow][i]} {sheet[_zeroBaseNameRow][i]};\n";
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
                GenerateFile(_dbGeneratedPath, $"{table.TableName}.json", text, true);
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

        var datas = new System.Object[table.Rows.Count - _zeroBaseDataStartedRow];
        var filedNames = table.Rows[_zeroBaseNameRow];
        var isSucceed = true;
        for (int i = 0; i < datas.Length; i++)
        {
            var data = table.Rows[_zeroBaseDataStartedRow + i];
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
            GenerateFile(_enumGeneratedPath, "Enum.cs", normalizedText, true);
        }
    }

    private List<string> GetEnumScriptText(DataRowCollection rows)
    {
        var sb = new StringBuilder();
        var arr = new List<string>(8);
        var template = "public enum {0}\n{{\n{1}}}\n";

        for (int i = _enumDataStartedRow - 1; i < rows.Count; i++)
        {
            var typeText = rows[i][_enumTypeColumn].ToString();

            sb.Clear();
            while (i < rows.Count && rows[i][_enumTypeColumn].ToString() == typeText)
            {
                // TODO : 매 키 마다 밸류는 적을 것이냐, 분기에만 적을 것이냐
                sb.Append($"\t{rows[i][_enumKeyColumn]} = {rows[i][_enumValueColumn]},");
                if (rows[i][_enumCommentsColumn].ToString() != string.Empty)
                {
                    sb.Append($" // {rows[i][_enumCommentsColumn]}");
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
        return (table.Rows[_zeroBaseNameRow][index].ToString().Length > 0 && table.Rows[_zeroBaseNameRow][index].ToString()[0] == '#');
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