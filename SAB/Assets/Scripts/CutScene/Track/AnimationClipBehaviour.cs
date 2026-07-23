using UnityEngine;
using UnityEngine.Playables;

namespace SAB.Cutscene
{
    public class AnimationClipBehaviour : PlayableBehaviour
    {
        public AnimationClip Clip;

        private bool _started;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_started)
                return;

            if (playerData is Character receiver)
            {
                Debug.Log("start");

                _started = true;
                receiver.PlayEmote(Clip);
            }
        }

        public override void OnGraphStop(Playable playable)
        {
            Debug.Log("end");

            _started = false;
        }
    }
}
