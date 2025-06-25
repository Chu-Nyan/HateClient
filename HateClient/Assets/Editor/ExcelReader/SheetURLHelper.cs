using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class SheetURLHelper : ScriptableObject
{
    private const string _googleDownloadURL = "https://docs.google.com/spreadsheets/d/{0}/export?format=xlsx";
    private string _googleSheetID;
    private int _dataNameRowNumber;
    private int _dataTypeRowNumber;
    private int _dataStartedRowNumber;
    private int _zeroBaseNameRow;
    private int _zeroBaseTypeRow;
    private int _zeroBaseDataStartedRow;

    private string _classGeneratedPath;
    private string _dbGeneratedPath;
    private bool _isAvoidDuplication;

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

    public bool IsAvoidDuplication
    {
        get => _isAvoidDuplication;
        set => _isAvoidDuplication = value;
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

        Debug.Log("생성 시작");
        var log = "클래스 생성 결과\n";
        var tables = _excelData.Tables;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < tables.Count; i++)
        {
            var type = assemblies
                .Select(a => a.GetType(tables[i].TableName))
                .FirstOrDefault(t => t != null);

            if (type == null || _isAvoidDuplication == false)
            {
                var text = GetClassScriptText(tables[i]);
                GenerateFile(_classGeneratedPath, $"{tables[i].TableName}.cs", text);
                if (_isAvoidDuplication == true)
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

    public void ConvertClassToJson(System.Data.DataTable table)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type type = assemblies
            .Select(a => a.GetType(table.TableName))
            .FirstOrDefault(t => t != null);

        var datas = new System.Object[table.Rows.Count - _zeroBaseDataStartedRow];
        var filedNames = table.Rows[_zeroBaseNameRow];
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
                Debug.LogError($"{table.TableName} 클래스가 생성되지 않음\n\n {err.Message}");
                return;
            }
        }

        var text = JsonConvert.SerializeObject(datas, Formatting.Indented);
        Debug.Log(text);
    }

    private bool IsPassRow(DataTable table, int index)
    {
        return (table.Rows[_zeroBaseNameRow][index].ToString().Length > 0 && table.Rows[_zeroBaseNameRow][index].ToString()[0] == '#');
    }

    private void GenerateFile(string path, string fileName, string text)
    {
        // TODO : Path가 무조건 Asset 내부에서만
        path = Path.Combine(path, fileName);
        File.AppendAllText(path, text);
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
