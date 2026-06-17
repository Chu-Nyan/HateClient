using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Chu.Tools
{
    [Serializable]
    public class GoogleSheetsLoader
    {
        private const string _googleDownloadURL = "https://docs.google.com/spreadsheets/d/{0}/export?format=xlsx";

        [SerializeField]
        private string _googleSheetID;
        private bool _isLoading = false;
        private DateTime _excelUpdateTime;
        private DataTableCollection _sheets;

        public DataTableCollection Sheets
        {
            get => _sheets;
        }

        public async Task RequestExcelFile()
        {
            _isLoading = true;
            var www = UnityWebRequest.Get(string.Format(_googleDownloadURL, _googleSheetID));
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError("실패: " + www.error);
            else
            {
                _excelUpdateTime = DateTime.Now;
                var stream = new MemoryStream(www.downloadHandler.data);
                _sheets = ExcelReaderFactory.CreateReader(stream).AsDataSet().Tables;
            }
            _isLoading = false;
        }

        public void DrawSheetSettingUI()
        {
            EditorGUILayout.LabelField("Google Sheets", EditorStyles.boldLabel);
            _googleSheetID = EditorGUILayout.TextField("Sheets ID", _googleSheetID);
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(_isLoading);
            if (GUILayout.Button(_isLoading ? "Downloading..." : "Request File", GUILayout.MaxWidth(150)))
                _ = RequestExcelFile();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.LabelField(Sheets != null ? $"Last Sync: {_excelUpdateTime}" : "No Data");
            EditorGUILayout.EndHorizontal();
        }
    }
}
