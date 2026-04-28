using UnityEditor;
using UnityEngine;
using System;
using Chu.Data;

namespace Chu.UnityEditor
{
    [CustomPropertyDrawer(typeof(IVariantParam), true)]
    public class VariantParamDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);
            float typeWidth = 100f; Rect typeRect = new(position.x, position.y, typeWidth, position.height);
            Rect valueRect = new(position.x + typeWidth + 5, position.y, position.width - typeWidth - 5, position.height);
            GUIContent typeLabel = new(GetTypeName(property));

            if (EditorGUI.DropdownButton(typeRect, typeLabel, FocusType.Passive))
            {
                ShowMenu(property);
            }

            if (property.managedReferenceValue != null)
            {
                var valueProp = property.FindPropertyRelative("value");
                if (valueProp != null)
                {
                    EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
                }
            }
        }

        private void ShowMenu(SerializedProperty prop)
        {
            var menu = new GenericMenu();

            AddType(menu, prop, "int", typeof(int));
            AddType(menu, prop, "float", typeof(float));
            AddType(menu, prop, "bool", typeof(bool));
            AddType(menu, prop, "string", typeof(string));
            AddType(menu, prop, "Vector2", typeof(Vector2));
            AddType(menu, prop, "Vector3", typeof(Vector3));
            AddType(menu, prop, "Component", typeof(Component));

            menu.ShowAsContext();
        }

        private void AddType(GenericMenu menu, SerializedProperty prop, string label, Type type)
        {
            menu.AddItem(new GUIContent(label), false, () => {
                var genericType = typeof(VariantParam<>).MakeGenericType(type);
                prop.managedReferenceValue = Activator.CreateInstance(genericType);
                prop.serializedObject.ApplyModifiedProperties();
            });
        }

        private string GetTypeName(SerializedProperty prop)
        {
            var obj = prop.managedReferenceValue;
            if (obj == null) return "None";

            var type = obj.GetType();

            if (!type.IsGenericType)
                return type.Name;

            var t = type.GetGenericArguments()[0];

            return t switch
            {
                Type x when x == typeof(int) => "int",
                Type x when x == typeof(float) => "float",
                Type x when x == typeof(bool) => "bool",
                Type x when x == typeof(string) => "string",
                Type x when x == typeof(Vector2) => "Vector2",
                Type x when x == typeof(Vector3) => "Vector3",
                Type x when x == typeof(Component) => "Component",
                _ => t.Name
            };
        }
    }
}