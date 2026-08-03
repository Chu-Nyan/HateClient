using UnityEngine;
using UnityEngine.Playables;

namespace SAB.Cutscene
{
    public class MoveClipBehaviour : PlayableBehaviour
    {
        public Vector3 Destination;

        private bool _started;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_started)
                return;

            if (playerData is IMovementReceiver receiver)
            {
                _started = true;
                receiver?.SetDestination(Destination);
            }
        }

        public override void OnGraphStop(Playable playable)
        {
            _started = false;
        }
    }
}
