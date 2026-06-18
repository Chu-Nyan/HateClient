using UnityEngine;
using UnityEngine.Playables;

public class AnimationClipAsset : InGameClip
{
    [SerializeField]
    private AnimationClip _clip;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimationClipBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.Clip = _clip;

        return playable;
    }
}
