using UnityEditor;
using UnityEngine;

namespace SAB.Cutscene
{
    [CustomEditor(typeof(CutsceneJsonConverter))]
    public class CutsceneJsonConverterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Export JSON"))
            {
                var trigger = (CutsceneJsonConverter)target;
                trigger.ExportJson();
            }
        }
    }
}
