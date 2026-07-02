using System;
using UnityEngine;
using UnityEngine.Playables;

public class MarkerReceiver : MonoBehaviour, INotificationReceiver
{
    private event Action<DialogMarker> DialogRequested;

    public void Init(Action<DialogMarker> dialogRequested)
    {
        DialogRequested = dialogRequested;
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogMarker marker)
        {
            DialogRequested?.Invoke(marker);
        }
    }
}
