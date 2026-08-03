using UnityEditor;
using UnityEngine;

namespace Chu.Tools
{
    [CustomEditor(typeof(TranslationDBHelper))]
    public class TranslationDBHelperEditor : Editor
    {
        private TranslationDBHelper _helper;
        private SerializedProperty _excelLoader;
        private SerializedProperty _config;

        private void OnEnable()
        {
            _helper = (TranslationDBHelper)target;
            _excelLoader = serializedObject.FindProperty("ExcelLoader");
            _config = serializedObject.FindProperty("Config");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_excelLoader, new GUIContent("Google Sheets"), true);
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
}