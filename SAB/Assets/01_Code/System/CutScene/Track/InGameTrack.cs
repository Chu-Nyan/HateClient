using UnityEngine;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    [TrackClipType(typeof(InGameClip))]
    [TrackBindingType(typeof(MonoBehaviour))]
    [TrackColor(1f, 1f, 1f)]
    public class InGameTrack : TrackAsset
    {
    }
}
