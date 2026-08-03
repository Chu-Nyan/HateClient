using SAB.DataManger;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    private int _humanID;
    private CharacterRepository _repo;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var character = (Character)target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("에디터 전용", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        _humanID = EditorGUILayout.IntField("외형 변경", _humanID);

        // 버튼
        if (GUILayout.Button("변경", GUILayout.Width(140)))
        {
            _repo ??= new();
            character.SetCustomizing(_repo.CustomizingData[_humanID]);
        }

        EditorGUILayout.EndHorizontal();
    }
}
