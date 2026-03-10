using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExcelHelper))]
public class ExcelHelperEditor : Editor
{
    private ExcelHelper _helper;
    private SerializedProperty _convertSetting;

    private void OnEnable()
    {
        _convertSetting = serializedObject.FindProperty("ConvertSetting");
        _helper = (ExcelHelper)target;
    }

    public override void OnInspectorGUI()
    {
        _helper.ExcelLoader.DrawSheetSettingUI();

        GUILayout.Space(10);
        if (GUILayout.Button("Export Data From DB"))
            _helper.SetupAllInOneAsync();

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(_convertSetting, new GUIContent("Config"), true);

        if (GUI.changed)
            EditorUtility.SetDirty(_helper);

        serializedObject.ApplyModifiedProperties();
    }
}

