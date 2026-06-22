using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogMarker : Marker, INotification, INotificationOptionProvider
{
    public int DialogID;
    public int SpeakerID;

    public PropertyName id => new("Dialog");

    public NotificationFlags flags
    {
        get => NotificationFlags.Retroactive | NotificationFlags.TriggerInEditMode;
    }
}
