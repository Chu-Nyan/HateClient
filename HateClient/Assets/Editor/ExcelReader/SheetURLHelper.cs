using System;
using System.Collections.Generic;
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

    [SerializeField, Header("구글 시트 ID")]
    private string _googleSheetID;
    [SerializeField, Header("변수 이름 행 번호")]
    private int _dataNameRowNumber;
    [SerializeField, Header("자료형 행 번호")]
    private int _dataTypeRowNumber;
    [SerializeField, Header("데이터 시작 행 번호")]
    private int _dataStartedRowNumber;
    [SerializeField, Header("클래스 생성 위치")]
    private string _classGeneratedPath = Application.dataPath;

    private int DataNameRowNumber
    {
        get => _dataNameRowNumber - 1;
    }

    private int DataTypeRowNumber
    {
        get => _dataTypeRowNumber - 1;
    }

    private int DataStartedRowNumber
    {
        get => _dataStartedRowNumber - 1;
    }

    [ContextMenu("Load")]
    private async void Load()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        Debug.Log("데이터 요청 중...");
        using var www = UnityWebRequest.Get(string.Format(_googleDownloadURL,_googleSheetID));
        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield(); // 메인 스레드 유지

        if (www.result == UnityWebRequest.Result.Success)
        {
            using var stream = new MemoryStream(www.downloadHandler.data);
            ReadExcel(stream);
        }
        else
        {
            Debug.LogError("실패: " + www.error);
        }
    }

    private void ReadExcel(MemoryStream data)
    {
        using (var reader = ExcelReaderFactory.CreateReader(data))
        {
            var result = reader.AsDataSet();

    private string GetClassScriptText(System.Data.DataTable table)
    {
        // 주석용 열 제거
        var exceptionColumns = new HashSet<int>();
        var sheet = table.Rows;
        for (int i = 0; i < table.Columns.Count; i++)
        {
            var name = sheet[DataNameRowNumber][i].ToString();
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

            code += $"\t public {sheet[DataTypeRowNumber][i]} {sheet[DataNameRowNumber][i]};\n";
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

        var datas = new System.Object[table.Rows.Count - DataStartedRowNumber];
        var filedNames = table.Rows[DataNameRowNumber];
        for (int i = 0; i < datas.Length; i++)
        {
            var data = table.Rows[DataStartedRowNumber + i];
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

    private bool IsPassRow(System.Data.DataTable table, int index)
    {
        return (table.Rows[DataNameRowNumber][index].ToString().Length > 0 && table.Rows[DataNameRowNumber][index].ToString()[0] == '#');
    }
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
