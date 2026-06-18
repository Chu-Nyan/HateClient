using UnityEngine;
using UnityEngine.Playables;

public class MoveClipAsset : InGameClip
{
    [SerializeField]
    private Vector3 _destination;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoveClipBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.Destination = _destination;

        return playable;
    }
}
