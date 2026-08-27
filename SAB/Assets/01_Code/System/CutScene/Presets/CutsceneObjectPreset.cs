using Chu.Data;
using SAB.Unit;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneObjectPreset : MonoBehaviour
    {
        public const string BindingSlot = "Binding Slot";

        public MonoBehaviour Target;
        public BindingSource BindingSource;
        public bool IsPersistent;
        public VariableParams VariantParams;

        [SerializeField, HideInInspector]
        private BindingSource _prevSource = BindingSource.SceneObject;

        private void OnValidate()
        {
            if (Target.gameObject != gameObject)
                Debug.LogError("Target must reference the same GameObject.");

            if (_prevSource == BindingSource)
                return;

            VariantParams.Clear();

            if (BindingSource == BindingSource.Slot)
            {
                VariantParams.Add(BindingSlot, ParameterType.UniqueEntity);
            }

            _prevSource = BindingSource;
        }

        public ICutscenePreset GetCutsceneObjectData(CutsceneDataExtractor idHandler)
        {
            if (Target == null)
                throw new System.Exception($"Target is null\nName : {gameObject.name}");
            if (Target is not ICutsceneObject)
                throw new System.Exception($"Target must implement\nName : {gameObject.name}");

            if (Target is VCamFollow follow)
            {
                if (follow.CinemachineFollow.FollowTarget == null)
                    throw new System.Exception($"VCam Follow Target is empty\nName : {gameObject.name}");

                var pov = follow.CinemachineCamera.Lens.FieldOfView;
                var targetID = idHandler.GetOrRegisterID(follow.CinemachineFollow.FollowTarget.gameObject);
                var followComp = follow.CinemachineFollow;
                return new VCamFollowData(transform.rotation, pov, targetID, followComp.FollowOffset, followComp.TrackerSettings.PositionDamping);
            }
            else if (Target is VCamStatic vcamStatic)
            {
                return new VCamStaticData(transform.position, transform.rotation, vcamStatic.CinemachineCamera.Lens.FieldOfView);
            }
            else if (Target is Character acter)
            {
                return new CharacterSpawnRequest(acter.Stats.CharacterID, acter.BrainType, transform.position, transform.rotation);
            }
            else
            {
                throw new System.Exception($"Target is not supported\nType : {Target.GetType().Name}");
            }
        }
    }
}
