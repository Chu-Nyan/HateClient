using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SheetURLHelper))]
public class SheetURLHelperEditor : Editor
{
    private bool _isSheetSettingFoldedOut = false;
    private bool _isEnumSettingFoldedOut = false;
    private bool _isDataClassSettingFoldedOut = false;
    private string _foldoutName = "설정";

    public override void OnInspectorGUI()
    {
        var helper = (SheetURLHelper)target;

        DrawSheetSettingUI(helper);
        GUILayout.Space(10);
        DrawEnumSettingUI(helper);
        GUILayout.Space(10);
        DrawGenerateClassTextUI(helper);
        GUILayout.Space(10);


        if (GUI.changed)
            EditorUtility.SetDirty(helper);
    }

    private void DrawSheetSettingUI(SheetURLHelper helper)
    {
        var tempNameRow = helper.DataNameRow + 1;
        var tempTypeRow = helper.DataTypeRow + 1;
        var tempStartedRow = helper.DataStartedRow + 1;

        EditorGUILayout.LabelField("구글 스프레드 시트 설정", EditorStyles.boldLabel);
        helper.GoogleSheetID = EditorGUILayout.TextField("시트 ID", helper.GoogleSheetID);
        _isSheetSettingFoldedOut = EditorGUILayout.Foldout(_isSheetSettingFoldedOut, _foldoutName);
        if (_isSheetSettingFoldedOut == true)
        {
            helper.DataNameRow = EditorGUILayout.IntField("이름 행", tempNameRow) - 1;
            helper.DataTypeRow = EditorGUILayout.IntField("타입 행", tempTypeRow) - 1;
            helper.DataStartedRow = EditorGUILayout.IntField("데이터 시작 행", tempStartedRow) - 1;
        }

        GUILayout.Space(5);
        if (GUILayout.Button("All-In-One 생성"))
            helper.SetupAllInOneAsync();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("엑셀 파일 요청", GUILayout.MaxWidth(200)))
            LoadExcel(helper);
        EditorGUILayout.LabelField(helper.HasExcelData == true ? $"업데이트 시간 : {helper.ExcelUpdateTime}" : $"데이터 없음");
        EditorGUILayout.EndHorizontal();
    }

    private void DrawEnumSettingUI(SheetURLHelper helper)
    {
        var tempStartedRow = helper.EnumDataStartedRow + 1;
        var tempTypeColumn = helper.EnumTypeColumn + 1;
        var tempKeyColumn = helper.EnumKeyColumn + 1;
        var tempValueColumn = helper.EnumValueColumn + 1;
        var tempCommentColumn = helper.EnumCommentsColumn + 1;

        EditorGUILayout.LabelField("Enum 생성 설정", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        helper.EnumGeneratedPath = EditorGUILayout.TextField("생성 경로", helper.EnumGeneratedPath);
        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            var selected = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, helper.EnumGeneratedPath);
            helper.EnumGeneratedPath = "Assets" + selected.Substring(Application.dataPath.Length);
        }
        EditorGUILayout.EndHorizontal();

        _isEnumSettingFoldedOut = EditorGUILayout.Foldout(_isEnumSettingFoldedOut, _foldoutName);
        if (_isEnumSettingFoldedOut == true)
        {
            helper.EnumDataStartedRow = EditorGUILayout.IntField("데이터 시작 행", tempStartedRow) - 1;
            helper.EnumTypeColumn = EditorGUILayout.IntField("타입 열", tempTypeColumn) - 1;
            helper.EnumKeyColumn = EditorGUILayout.IntField("키 열", tempKeyColumn) - 1;
            helper.EnumValueColumn = EditorGUILayout.IntField("밸류 열", tempValueColumn) - 1;
            helper.EnumCommentsColumn = EditorGUILayout.IntField("주석 열", tempCommentColumn) - 1;
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Enum 스크립트 생성"))
        {
            helper.GenerateEnumScript();
            AssetDatabase.Refresh();
        }
    }


    private void DrawGenerateClassTextUI(SheetURLHelper helper)
    {
        EditorGUILayout.LabelField("파일 생성 설정", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        helper.ClassGeneratedPath = EditorGUILayout.TextField("클래스 생성 경로", helper.ClassGeneratedPath);
        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            var selected = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, helper.ClassGeneratedPath);
            helper.ClassGeneratedPath = "Assets" + selected.Substring(Application.dataPath.Length);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        helper.DBGeneratedPath = EditorGUILayout.TextField("DB 생성 경로", helper.DBGeneratedPath);
        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            var selected = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, helper.EnumGeneratedPath);
            helper.DBGeneratedPath = "Assets" + selected.Substring(Application.dataPath.Length);
        }
        EditorGUILayout.EndHorizontal();

        _isDataClassSettingFoldedOut = EditorGUILayout.Foldout(_isDataClassSettingFoldedOut, _foldoutName);
        if (_isDataClassSettingFoldedOut == true)
        {
            helper.IsClassAvoidDuplication = EditorGUILayout.Toggle("클래스 중복 방지", helper.IsClassAvoidDuplication);
        }

        GUILayout.Space(5);
        DrawGenerateClassFuntion(helper);

        void DrawGenerateClassFuntion(SheetURLHelper helper)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("데이터 스크립트 생성"))
            {
                helper.GenerateClass();
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("DB를 Json으로 생성"))
            {
                helper.GenerateDBJson();
                AssetDatabase.Refresh();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private async void LoadExcel(SheetURLHelper helper)
    {
        await helper.LoadExcelFile();
    }
}
