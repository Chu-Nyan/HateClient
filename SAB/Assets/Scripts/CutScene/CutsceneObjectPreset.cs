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

        public CutsceneObjectType Type;
        public BindingSource BindingSource;
        public bool IsPersistent;
        public VariableParams VariantParams;

        [SerializeField, HideInInspector]
        private CutsceneObjectType _prevType = CutsceneObjectType.SingleMesh;
        [SerializeField, HideInInspector]
        private BindingSource _prevSource = BindingSource.SceneObject;

        private void OnValidate()
        {
            if (_prevSource == BindingSource && _prevType == Type)
                return;
            if (VariantParams == null)
                VariantParams = new();

            VariantParams.Clear();

            if (BindingSource == BindingSource.SceneObject)
            {
                VariantParams.Add(_bindingObject, ParameterType.Script);
            }
            else if (BindingSource == BindingSource.Slot)
            {
                VariantParams.Add(_bindingSlot, ParameterType.UniqueEntity);
            }

            _prevType = Type;
            _prevSource = BindingSource;
        }

        public ICutscenePreset GetCutsceneObjectData(CutsceneJsonConverter idHandler)
        {
            if (Type == CutsceneObjectType.SingleMesh)
            {
                var singlemesh = GetComponent<SingleMesh>();
                string path = Utility.GetAddressablePath(singlemesh.MeshFilter.sharedMesh);
                return new SingleMeshData(path, transform.position, transform.rotation);
            }
            else if (Type == CutsceneObjectType.VCamFollow)
            {
                var vcam = GetComponent<VCamFollow>();
                var pov = vcam.CinemachineCamera.Lens.FieldOfView;
                var targetID = idHandler.GetOrRegisterID(vcam.CinemachineFollow.FollowTarget.gameObject);
                return new VCamFollowData(transform.rotation, pov, targetID, vcam.CinemachineFollow.FollowOffset);
            }
            else if (Type == CutsceneObjectType.VCamStatic)
            {
                var vcam = GetComponent<VCamStatic>();
                return new VCamStaticData(transform.position, transform.rotation, vcam.CinemachineCamera.Lens.FieldOfView);
            }
            else if (Type == CutsceneObjectType.Character)
            {
                var acter = GetComponent<Character>();
                return new SpawnRequest(acter.Stats.CharacterID, acter.BrainType, transform.position, transform.rotation);
            }

            throw new System.Exception($"{gameObject.name}: is not {Type}");
        }
    }
}
