using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace SAB.Cutscene
{
    [CustomEditor(typeof(CutsceneDirector))]
    public class CutsceneAuthoringEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var authoring = (CutsceneDirector)target;

            if (authoring.PlayableDirector == null)
            {
                EditorGUILayout.Space();

                EditorGUILayout.HelpBox(
                    "PlayableDirector component required to play cutscene.",
                    MessageType.Error
                );

                if (GUILayout.Button("Add PlayableDirector"))
                {
                    Undo.AddComponent<PlayableDirector>(authoring.gameObject);
                }
            }
        }
    }
}
