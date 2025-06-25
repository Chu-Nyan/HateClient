using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SheetURLHelper))]
public class SheetURLHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var helper = (SheetURLHelper)target;

        DrawSheetSettingUI(helper);
        GUILayout.Space(10);
        DrawGenerateClassTextUI(helper);
        GUILayout.Space(10);
        DrawControlFuntion(helper);

        if (GUI.changed)
            EditorUtility.SetDirty(helper);
    }

    private void DrawSheetSettingUI(SheetURLHelper helper)
    {
        EditorGUILayout.LabelField("구글 스프레드 시트 설정", EditorStyles.boldLabel);
        helper.GoogleSheetID = EditorGUILayout.TextField("시트 ID", helper.GoogleSheetID);
        helper.DataNameRowNumber = EditorGUILayout.IntField("이름 행 번호", helper.DataNameRowNumber);
        helper.DataTypeRowNumber = EditorGUILayout.IntField("타입 행 번호", helper.DataTypeRowNumber);
        helper.DataStartedRowNumber = EditorGUILayout.IntField("데이터 시작 행 번호", helper.DataStartedRowNumber);
    }

    private void DrawGenerateClassTextUI(SheetURLHelper helper)
    {
        EditorGUILayout.LabelField("파일 생성 설정", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        helper.ClassGeneratedPath = EditorGUILayout.TextField("클래스 생성 경로", helper.ClassGeneratedPath);
        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            var selected = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
            helper.ClassGeneratedPath = "Assets" + selected.Substring(Application.dataPath.Length);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        helper.DBGeneratedPath = EditorGUILayout.TextField("DB 생성 경로", helper.DBGeneratedPath);
        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            var selected = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
            helper.DBGeneratedPath = "Assets" + selected.Substring(Application.dataPath.Length);
        }
        EditorGUILayout.EndHorizontal();

        helper.IsClassAvoidDuplication = EditorGUILayout.Toggle("클래스 중복 방지", helper.IsClassAvoidDuplication);
    }

    private void DrawControlFuntion(SheetURLHelper helper)
    {
        EditorGUILayout.LabelField("기능", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("엑셀 파일 요청", GUILayout.MaxWidth(200)))
            LoadExcel(helper);
        EditorGUILayout.LabelField(helper.HasExcelData == true ? $"업데이트 시간 : {helper.ExcelUpdateTime}" : $"데이터 없음");
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("클래스 파일 생성", GUILayout.MaxWidth(200)))
        {
            helper.GenerateClass();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("DB를 Json으로 생성", GUILayout.MaxWidth(200)))
        {
            helper.GenerateDBJson();
            AssetDatabase.Refresh();
        }
    }

    private async void LoadExcel(SheetURLHelper helper)
    {
        await helper.LoadExcelFile();
    }
}
