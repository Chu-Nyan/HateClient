using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    public class EventMarker : Marker, INotification, INotificationOptionProvider
    {
        public string Action;
        public string Target;
        public float Time;

        public PropertyName id => new("Event");

        public NotificationFlags flags
        {
            get => NotificationFlags.Retroactive | NotificationFlags.TriggerInEditMode;
        }
    }
}
