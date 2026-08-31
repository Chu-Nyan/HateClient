using Chu.Collision;
using UnityEngine;
using UnityEngine.Playables;

namespace SAB.Cutscene
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CutscenePreset : MonoBehaviour
    {
        [SerializeField]
        public PlayableDirector PlayableDirector;
        [SerializeField]
        public bool LockPlayer;
        [SerializeField]
        public ShapeParam[] TriggerZones;

        public void OnDrawGizmos()
        {
            if (TriggerZones == null)
                return;

            for (int i = 0; i < TriggerZones.Length; i++)
            {
                TriggerZones[i].DrawGizmo(transform.position, transform.rotation);
            }
        }

        public CutsceneTriggerData ToData()
        {
            if (TriggerZones.Length == 0)
                throw new System.Exception($"Trigger bounds not defined.\nName : {gameObject.name}");

            return new()
            {
                Name = PlayableDirector.playableAsset.name,
                CenterX = transform.position.x,
                CenterY = transform.position.z,
                TriggerZones = TriggerZones
            };
        }
    }
}
