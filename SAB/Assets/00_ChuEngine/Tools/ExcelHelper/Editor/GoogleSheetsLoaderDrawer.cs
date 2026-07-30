using UnityEditor;
using UnityEngine;

namespace Chu.Tools
{
    [CustomPropertyDrawer(typeof(GoogleSheetsLoader))]
    public class GoogleSheetsLoaderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var loader = fieldInfo.GetValue(property.serializedObject.targetObject) as GoogleSheetsLoader;
            float line = EditorGUIUtility.singleLineHeight;
            Rect idRect = new(position.x, position.y, position.width, line);
            Rect buttonRect = new(position.x, position.y + line + 5, 120, line);
            Rect stateRect = new(position.x + 130, position.y + line + 5, position.width - 130, line);

            EditorGUI.BeginProperty(position, label, property);
            loader.GoogleSheetID = EditorGUI.TextField(idRect, "Sheet ID", loader.GoogleSheetID);
            EditorGUI.BeginDisabledGroup(loader.IsLoading);
            if (GUI.Button(buttonRect, loader.IsLoading ? "Downloading..." : "Request File"))
                _ = loader.RequestExcelFile();

            EditorGUI.EndDisabledGroup();
            EditorGUI.LabelField(stateRect, loader.Sheets != null ? $"Last Sync : {loader.ExcelUpdateTime}" : "No Data");
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + 5;
        }
    }
}