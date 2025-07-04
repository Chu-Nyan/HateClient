using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SheetURLHelper))]
public class SheetURLHelperEditor : Editor
{
    private bool _isCommonSettingFoldedOut = false;
    private bool _isDataClassSettingFoldedOut = false;
    private string _foldoutName = "설정";

    public override void OnInspectorGUI()
    {
        var helper = (SheetURLHelper)target;
        DrawCommonSettingUI(helper);
        GUILayout.Space(10);
        DrawSheetSettingUI(helper);
        GUILayout.Space(10);
        DrawEnumSettingUI(helper);
        GUILayout.Space(10);
        DrawGenerateClassTextUI(helper);
        GUILayout.Space(10);
        DrawGenerateLocalization(helper);

        if (GUI.changed)
            EditorUtility.SetDirty(helper);
    }

    private void DrawSheetSettingUI(SheetURLHelper helper)
    {
        EditorGUILayout.LabelField("구글 스프레드 시트 설정", EditorStyles.boldLabel);
        helper.GoogleSheetID = EditorGUILayout.TextField("시트 ID", helper.GoogleSheetID);

        GUILayout.Space(5);
        if (GUILayout.Button("All-In-One 생성"))
            helper.SetupAllInOneAsync();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("엑셀 파일 요청", GUILayout.MaxWidth(200)))
            LoadExcel(helper);
        EditorGUILayout.LabelField(helper.HasExcelData == true ? $"업데이트 시간 : {helper.ExcelUpdateTime}" : $"데이터 없음");
        EditorGUILayout.EndHorizontal();
    }

    private void DrawCommonSettingUI(SheetURLHelper helper)
    {
        EditorGUILayout.LabelField("공통 설정", EditorStyles.boldLabel);
        _isCommonSettingFoldedOut = EditorGUILayout.Foldout(_isCommonSettingFoldedOut, _foldoutName);
        if (_isCommonSettingFoldedOut == true)
        {
            EditorGUILayout.BeginHorizontal();
            helper.EditorGeneratedPath = EditorGUILayout.TextField("생성 경로", helper.EditorGeneratedPath);
            if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
            {
                var selected = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, helper.EditorGeneratedPath);
                helper.EditorGeneratedPath = "Assets" + selected.Substring(Application.dataPath.Length);
            }
            EditorGUILayout.EndHorizontal();

        }
    }

    private void DrawEnumSettingUI(SheetURLHelper helper)
    {

        EditorGUILayout.LabelField("Enum 생성 설정", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        helper.EnumGeneratedPath = EditorGUILayout.TextField("생성 경로", helper.EnumGeneratedPath);
        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            var selected = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, helper.EnumGeneratedPath);
            helper.EnumGeneratedPath = "Assets" + selected.Substring(Application.dataPath.Length);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        if (GUILayout.Button("Enum 스크립트 생성"))
        {
            GenerateEnumScript(helper);
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
                GenerateClass(helper);
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("DB를 Json으로 생성"))
            {
                GenerateDBJson(helper);
                AssetDatabase.Refresh();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawGenerateLocalization(SheetURLHelper helper)
    {
        EditorGUILayout.LabelField("텍스트 생성 설정", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        helper.LocalizationGeneratedPath = EditorGUILayout.TextField("생성 경로", helper.LocalizationGeneratedPath);
        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            var selected = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, helper.LocalizationGeneratedPath);
            helper.LocalizationGeneratedPath = "Assets" + selected.Substring(Application.dataPath.Length);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        if (GUILayout.Button("텍스트 생성"))
        {
            helper.ConvertLocalization();
            AssetDatabase.Refresh();
        }
    }

    private async void LoadExcel(SheetURLHelper helper)
    {
        await helper.LoadExcelFile();
    }

    private async void GenerateEnumScript(SheetURLHelper helper)
    {
        await helper.GenerateEnumScript();
    }

    private async void GenerateClass(SheetURLHelper helper)
    {
        await helper.GenerateClass();
    }

    private async void GenerateDBJson(SheetURLHelper helper)
    {
        await helper.GenerateDBJson();
    }
}
