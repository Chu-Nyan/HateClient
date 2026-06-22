using SAB.Cutscene;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MarkerReceiver : MonoBehaviour, INotificationReceiver
{
    private Dictionary<int, ICutsceneObject> _objects;

    public void Init(Dictionary<int, ICutsceneObject> objects)
    {
        _objects = objects;
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogMarker marker)
        {
            Debug.Log($"{marker.SpeakerID}, {marker.DialogID}");
        }
    }
}
