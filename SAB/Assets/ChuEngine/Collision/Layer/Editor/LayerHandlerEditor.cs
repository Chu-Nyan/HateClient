using UnityEditor;
using UnityEngine;

namespace Chu.Collision.Layer
{
    [CustomEditor(typeof(LayerHandler))]
    public class LayerHandlerEditor : Editor
    {
        private SerializedProperty _layerProp;

        private void OnEnable()
        {
            _layerProp = serializedObject.FindProperty("_layer");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var hander = (LayerHandler)target;

            EditorGUILayout.LabelField("레이어", EditorStyles.boldLabel);
            DrawLayerList();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
            DrawSaveAndDebug(hander);
        }

        private void DrawLayerList()
        {
            for (int i = 0; i < _layerProp.arraySize; i++)
            {
                var element = _layerProp.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Layer {i}", GUILayout.Width(70));
                element.stringValue = EditorGUILayout.TextField(element.stringValue);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawSaveAndDebug(LayerHandler handler)
        {
            bool isValid = handler.TryVerify(out string log);
            EditorGUILayout.HelpBox(log, isValid ? MessageType.Info : MessageType.Error);

            EditorGUI.BeginDisabledGroup(!isValid);
            if (GUILayout.Button("저장", GUILayout.Height(30)) == true)
            {
                GenerateScriptFile(handler);
                AssetDatabase.Refresh();
            }
            EditorGUI.EndDisabledGroup();
        }

        private void GenerateScriptFile(LayerHandler handler)
        {
            if (handler.GenerateFile(out string generateLog) == true)
                Debug.Log(generateLog);
            else
                Debug.LogError(generateLog);
        }
    }
}
