using Chu.Collision;
using UnityEngine;
using UnityEngine.Playables;

namespace SAB.Cutscene
{
    public class CutsceneAuthoring : MonoBehaviour
    {
        public ShapeParam[] _triggerZones;

        public PlayableDirector PlayableDirector
        {
            get => GetComponent<PlayableDirector>();
        }

        public Vector2 Center
        {
            get => new(transform.position.x, transform.position.z);
        }

        public ShapeParam[] TriggerZone
        {
            get => _triggerZones;
        }

        public void OnDrawGizmos()
        {
            if (_triggerZones == null)
                return;

            for (int i = 0; i < _triggerZones.Length; i++)
            {
                _triggerZones[i].DrawGizmo(transform.position, transform.rotation);
            }
        }
    }
}
