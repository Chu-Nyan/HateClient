using Chu.Data;
using UnityEditor;
using UnityEngine;

namespace Chu.UnityEditor
{
    [CustomPropertyDrawer(typeof(VariantParamPair))]
    public class VariantParamPairDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var keyProp = property.FindPropertyRelative("Key");
            var paramProp = property.FindPropertyRelative("Param");

            float keyWidth = 150f;

            var keyRect = new Rect(position.x, position.y, keyWidth, EditorGUIUtility.singleLineHeight);
            var valueRect = new Rect(position.x + keyWidth + 4, position.y,
                position.width - keyWidth - 4, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);

            if (paramProp.managedReferenceValue != null)
            {
                var value = paramProp.FindPropertyRelative("Value");
                EditorGUI.PropertyField(valueRect, value, GUIContent.none);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
