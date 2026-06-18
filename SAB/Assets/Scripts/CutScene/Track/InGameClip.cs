using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public abstract class InGameClip : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps => ClipCaps.None;
}
