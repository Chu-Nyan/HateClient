using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    [CustomEditor(typeof(DialogMarker))]
    public class DialogMarkerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty TextIDProp = serializedObject.FindProperty("TextID");
            SerializedProperty speakerTrackProp = serializedObject.FindProperty("SpeakerTrack");
            SerializedProperty timeProp = serializedObject.FindProperty("Time");

            serializedObject.Update();

            EditorGUILayout.PropertyField(TextIDProp, new GUIContent("TextID"));
            EditorGUILayout.PropertyField(timeProp, new GUIContent("Time"));

            if (target is Marker marker && marker.parent != null)
            {
                TimelineAsset timelineAsset = marker.parent.timelineAsset;
                List<TrackAsset> allTracks = timelineAsset.GetOutputTracks().ToList();

                if (allTracks.Count > 0)
                {
                    string[] trackNames = allTracks.Select(t => t.name).ToArray();
                    int currentIndex = System.Array.IndexOf(trackNames, speakerTrackProp.stringValue);
                    if (currentIndex == -1) currentIndex = 0;

                    int newIndex = EditorGUILayout.Popup("Speaker Track", currentIndex, trackNames);

                    if (newIndex >= 0 && newIndex < trackNames.Length)
                    {
                        speakerTrackProp.stringValue = trackNames[newIndex];
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("타임라인에 트랙이 없습니다.", MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.PropertyField(speakerTrackProp, new GUIContent("Speaker Track"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
