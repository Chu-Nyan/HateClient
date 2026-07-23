using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    [System.Serializable]
    public abstract class InGameClip : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps => ClipCaps.None;
    }
}
