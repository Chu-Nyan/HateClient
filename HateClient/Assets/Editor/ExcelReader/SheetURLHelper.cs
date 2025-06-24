using System.IO;
using System.Threading.Tasks;
using ExcelDataReader;
using UnityEngine;
using UnityEngine.Networking;

public class SheetURLHelper : ScriptableObject
{
    [SerializeField, Header("구글시트 주소")]
    private string _googleSheetID;
    private const string _googleDownloadURL = "https://docs.google.com/spreadsheets/d/{0}/export?format=xlsx";

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

            PrintExcelData(result.Tables);
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