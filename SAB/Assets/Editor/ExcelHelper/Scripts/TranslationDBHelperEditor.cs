using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TranslationDBHelper))]
public class TranslationDBHelperEditor : Editor
{
    private SerializedProperty _config;
    private TranslationDBHelper _helper;
    private void OnEnable()
    {
        _helper = (TranslationDBHelper)target;
        _config = serializedObject.FindProperty("Config");
    }

    public override void OnInspectorGUI()
    {
        _helper.ExcelLoader.DrawSheetSettingUI();
        EditorGUILayout.Space();
        DrawGenerateLocalization();
        EditorGUILayout.Space();
        DrawConfig();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawGenerateLocalization()
    {
        GUILayout.Space(5);
        if (GUILayout.Button("Export DB To Json"))
        {
            _helper.ExportDbToJson();
            AssetDatabase.Refresh();
        }
    }

    private void DrawConfig()
    {
        EditorGUILayout.PropertyField(_config, new GUIContent("Config"), true);
    }
}
