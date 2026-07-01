using SAB.Cutscene;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MarkerReceiver : MonoBehaviour, INotificationReceiver
{
    private Dictionary<string, int> _trackByID;
    private Func<int, ICutsceneObject> _getObjectFunc;

    public void Setup(Dictionary<string, int> trackByID, Func<int, ICutsceneObject> getObjectFunc)
    {
        _trackByID = trackByID;
        _getObjectFunc = getObjectFunc;
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogMarker marker)
        {
            if (_trackByID.TryGetValue(marker.SpeakerTrack, out int id) == false)
            {
                Debug.LogWarning($"{marker.TextID}, {marker.SpeakerTrack} is null");
                return;
            }

            if (_getObjectFunc(id) is not ISpeachable able)
            {
                Debug.LogWarning($"{_getObjectFunc(id)} is not ISpeachable");
                return;
            }

            able.Speech(new(0, marker.TextID, marker.Time), true);
        }
    }
}
