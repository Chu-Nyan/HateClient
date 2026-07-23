using Chu.Data;
using Chu.Utility.Unity;
using SAB.Unit;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneObjectPreset : MonoBehaviour
    {
        public const string _bindingObject = "Binding Object";
        public const string _bindingSlot = "Binding Slot";

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
                VariantParams.Add(_bindingSlot, ParameterType.UniqueEntity);
            }

            _prevSource = BindingSource;
        }

        public ICutscenePreset GetCutsceneObjectData(CutsceneJsonConverter idHandler)
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
            else if (Target is Character acter)
            {
                return new SpawnRequest(acter.Stats.CharacterID, acter.BrainType, transform.position, transform.rotation);
            }

            throw new System.Exception($"{gameObject.name}, {Target.GetType().Name}: not supported");
        }
    }
}
