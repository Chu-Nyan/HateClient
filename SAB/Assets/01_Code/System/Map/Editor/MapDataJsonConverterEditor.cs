using UnityEditor;
using UnityEngine;

namespace SAB.GameSystem
{
    [CustomEditor(typeof(MapDataJsonConverter))]
    public class MapDataJsonConverterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Convert Data to JSON", GUILayout.Height(30)))
            {
                MapDataJsonConverter converter = (MapDataJsonConverter)target;
                converter.Convert();
                EditorUtility.SetDirty(converter);
            }
        }
    }
}
