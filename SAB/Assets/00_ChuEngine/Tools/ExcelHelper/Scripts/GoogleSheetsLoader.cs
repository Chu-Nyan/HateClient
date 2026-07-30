using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Chu.Tools
{
    [Serializable]
    public class GoogleSheetsLoader
    {
        private const string GoogleDownloadURL = "https://docs.google.com/spreadsheets/d/{0}/export?format=xlsx";
        public const char IgnoreSymbol = '#';

        public string GoogleSheetID;
        [NonSerialized]
        public bool IsLoading;
        [NonSerialized]
        public DateTime ExcelUpdateTime;
        [NonSerialized]
        public DataTableCollection Sheets;

        public async Task RequestExcelFile()
        {
            if (IsLoading == true)
                return;

            IsLoading = true;
            var www = UnityWebRequest.Get(string.Format(GoogleDownloadURL, GoogleSheetID));
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError("실패: " + www.error);
            else
            {
                ExcelUpdateTime = DateTime.Now;
                var stream = new MemoryStream(www.downloadHandler.data);
                Sheets = ExcelReaderFactory.CreateReader(stream).AsDataSet().Tables;
            }
            IsLoading = false;
        }

        public static bool HasIgnoreSymbol(string text)
        {
            return text.Length == 0 || text[0] == IgnoreSymbol;
        }
    }
}
