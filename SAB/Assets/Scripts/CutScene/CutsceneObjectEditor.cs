using UnityEditor;

namespace SAB.Cutscene
{
    [CustomEditor(typeof(CutsceneObject))]
    public class CutsceneObjectEditor : Editor
    {
        SerializedProperty _type;
        SerializedProperty _bindingSource;
        SerializedProperty _bindingSlot;
        SerializedProperty _bindingObject;
        SerializedProperty _variantParam;

        private void OnEnable()
        {
            _type = serializedObject.FindProperty(nameof(CutsceneObject.Type));
            _bindingSource = serializedObject.FindProperty(nameof(CutsceneObject.BindingSource));
            _bindingObject = serializedObject.FindProperty(nameof(CutsceneObject.BindingObject));
            _bindingSlot = serializedObject.FindProperty(nameof(CutsceneObject.BindingSlot));
            _variantParam = serializedObject.FindProperty("_variantParam");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_type);
            EditorGUILayout.PropertyField(_bindingSource);

            switch ((BindingSource)_bindingSource.enumValueIndex)
            {
                case BindingSource.SceneObject:
                    EditorGUILayout.PropertyField(_bindingObject);
                    break;
                case BindingSource.Slot:
                    EditorGUILayout.PropertyField(_bindingSlot);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_variantParam, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

