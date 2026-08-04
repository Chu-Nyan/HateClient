using Chu.Collision;
using UnityEngine;
using UnityEngine.Playables;

namespace SAB.Cutscene
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CutscenePreset : MonoBehaviour
    {
        public bool LockPlayer;
        public ShapeParam[] TriggerZones;

        public PlayableDirector PlayableDirector
        {
            get => GetComponent<PlayableDirector>();
        }

        public Vector2 Center
        {
            get => new(transform.position.x, transform.position.z);
        }

        public void OnDrawGizmos()
        {
            if (TriggerZones == null)
                return;

            for (int i = 0; i < TriggerZones.Length; i++)
            {
                TriggerZones[i].DrawGizmo(transform.position, transform.rotation);
            }
        }
    }
}
