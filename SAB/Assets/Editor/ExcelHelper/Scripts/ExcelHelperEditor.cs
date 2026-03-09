using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExcelHelper))]
public class ExcelHelperEditor : Editor
{
    private bool _isDataClassSettingFoldedOut = false;
    private SerializedProperty _convertSetting;
    private SerializedProperty _pathSetting;
    private string _foldoutName = "설정";

    private void OnEnable()
    {
        var helper = (ExcelHelper)target;
        _convertSetting = serializedObject.FindProperty("ConvertSetting");
        _pathSetting = serializedObject.FindProperty("GeneratePath");
    }

    public override void OnInspectorGUI()
    {
        var helper = (ExcelHelper)target;
        DrawSheetSettingUI(helper);
        GUILayout.Space(10);
        DrawEnumSettingUI(helper);
        GUILayout.Space(10);
        DrawGenerateClassTextUI(helper);
        GUILayout.Space(10);
        DrawGenerateLocalization(helper);
        GUILayout.Space(10);
        DrawConvertOption();

        if (GUI.changed)
            EditorUtility.SetDirty(helper);

        serializedObject.ApplyModifiedProperties();

    }

    private void DrawSheetSettingUI(ExcelHelper helper)
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

    private void DrawEnumSettingUI(ExcelHelper helper)
    {
        if (GUILayout.Button("Enum 스크립트 생성"))
        {
            GenerateEnumScript(helper);
            AssetDatabase.Refresh();
        }
    }

    private void DrawGenerateClassTextUI(ExcelHelper helper)
    {
        EditorGUILayout.LabelField("파일 생성 설정", EditorStyles.boldLabel);

        _isDataClassSettingFoldedOut = EditorGUILayout.Foldout(_isDataClassSettingFoldedOut, _foldoutName);
        if (_isDataClassSettingFoldedOut == true)
        {
            helper.IsClassAvoidDuplication = EditorGUILayout.Toggle("클래스 중복 방지", helper.IsClassAvoidDuplication);
        }

        GUILayout.Space(5);
        DrawGenerateClassFuntion(helper);

        void DrawGenerateClassFuntion(ExcelHelper helper)
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

    private void DrawGenerateLocalization(ExcelHelper helper)
    {
        EditorGUILayout.LabelField("텍스트 생성 설정", EditorStyles.boldLabel);
        GUILayout.Space(5);
        if (GUILayout.Button("텍스트 생성"))
        {
            helper.ConvertLocalization();
            AssetDatabase.Refresh();
        }
    }

    private void DrawConvertOption()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("엑셀 설정", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_convertSetting, new GUIContent("시트 레이아웃"), true);
        EditorGUILayout.PropertyField(_pathSetting, new GUIContent("생성 경로"), true);
    }

    private async void LoadExcel(ExcelHelper helper)
    {
        await helper.LoadExcelFile();
    }

    private async void GenerateEnumScript(ExcelHelper helper)
    {
        await helper.GenerateEnumScript();
    }

    private async void GenerateClass(ExcelHelper helper)
    {
        await helper.GenerateClass();
    }

    private async void GenerateDBJson(ExcelHelper helper)
    {
        await helper.GenerateDBJson();
    }
}
