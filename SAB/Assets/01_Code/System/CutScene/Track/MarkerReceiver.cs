using System;
using UnityEngine;
using UnityEngine.Playables;

namespace SAB.Cutscene
{
    public class MarkerReceiver : MonoBehaviour, INotificationReceiver
    {
        private event Action<EventMarker> DialogRequested;

        public void Init(Action<EventMarker> dialogRequested)
        {
            DialogRequested = dialogRequested;
        }

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is EventMarker marker)
            {
                DialogRequested?.Invoke(marker);
            }
        }
    }
}
