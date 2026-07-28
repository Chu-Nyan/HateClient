using Chu.Data;
using Chu.Utility.Unity;
using SAB.Unit;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneObjectPreset : MonoBehaviour
    {
        public const string BindingObject = "Binding Object";
        public const string BindingSlot = "Binding Slot";

        public MonoBehaviour Target;
        public BindingSource BindingSource;
        public bool IsPersistent;
        public VariableParams VariantParams;

        [SerializeField, HideInInspector]
        private BindingSource _prevSource = BindingSource.SceneObject;

        private void OnValidate()
        {
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
            if (Target is SingleMesh singlemesh)
            {
                string path = Utility.GetAddressablePath(singlemesh.MeshFilter.sharedMesh);
                return new SingleMeshData(path, transform.position, transform.rotation);
            }
            else if (Target is VCamFollow follow)
            {
                var pov = follow.CinemachineCamera.Lens.FieldOfView;
                var targetID = idHandler.GetOrRegisterID(follow.CinemachineFollow.FollowTarget.gameObject);
                return new VCamFollowData(transform.rotation, pov, targetID, follow.CinemachineFollow.FollowOffset);
            }
            else if (Target is VCamStatic vcamStatic)
            {
                return new VCamStaticData(transform.position, transform.rotation, vcamStatic.CinemachineCamera.Lens.FieldOfView);
            }
            else if (Target is global::Character acter)
            {
                return new CharacterSpawnRequest(acter.Stats.CharacterID, acter.BrainType, transform.position, transform.rotation);
            }

            throw new System.Exception($"{gameObject.name}, {Target.GetType().Name}: not supported");
        }
    }
}
