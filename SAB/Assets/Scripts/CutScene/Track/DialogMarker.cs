using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogMarker : Marker, INotification, INotificationOptionProvider
{
    public string TextID;
    public string SpeakerTrack;
    public float Time;

    public PropertyName id => new("Dialog");

    public NotificationFlags flags
    {
        get => NotificationFlags.Retroactive | NotificationFlags.TriggerInEditMode;
    }
}
