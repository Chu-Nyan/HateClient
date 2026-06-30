using Chu.Data;
using Chu.Utility.Unity;
using SAB.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneObjectPreset : MonoBehaviour
    {
        public const string _meshFilter = "MeshFilter";
        public const string _vcam = "VCam";
        public const string _vcamFollow = "VCamFollow";
        public const string _npcID = "NPCID";

        public CutsceneObjectType Type;
        private int _objectID;
        public BindingSource BindingSource;
        public GameObject BindingObject;
        public UniqueEntityType BindingSlot;
        [SerializeField]
        private List<VariantParamPair> _variantParam;

        [SerializeField, HideInInspector]
        private CutsceneObjectType _prevType = CutsceneObjectType.SingleMesh;

        public int ObjectID
        {
            get => _objectID;
            set => _objectID = value;
        }

        public Vector3 Position
        {
            get => transform.position;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
        }

        private void OnValidate()
        {
            if (Type == _prevType)
                return;

            _prevType = Type;

            if (_variantParam == null)
                _variantParam = new();
            else
                _variantParam.Clear();

            if (Type == CutsceneObjectType.SingleMesh)
            {
                _variantParam.Add(new VariantParamPair(_meshFilter, new VariantParam<MeshFilter>()));
            }
            else if (Type == CutsceneObjectType.VCamFollow)
            {
                _variantParam.Add(new VariantParamPair(_vcam, new VariantParam<Component>()));
                _variantParam.Add(new VariantParamPair(_vcamFollow, new VariantParam<Component>()));
            }
            else if (Type == CutsceneObjectType.VCamStatic)
            {
                _variantParam.Add(new VariantParamPair(_vcam, new VariantParam<Component>()));
            }
            else if (Type == CutsceneObjectType.Character)
            {
                _variantParam.Add(new VariantParamPair(_npcID, new VariantParam<int>()));
            }
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
                // id 수정 필요
                //var acter = GetComponent<Character>();
                var id = GetVariantParam<int>(_npcID);
                return new SpawnRequest(id, transform.position, transform.rotation);
            }

            throw new System.Exception($"{gameObject.name}: is not {Type}");
        }

        private T GetVariantParam<T>(string key)
        {
            foreach (var item in _variantParam)
            {
                if (item.Key != key)
                    continue;

                if (item.Param.GetValue() is not T value)
                    throw new System.Exception($"{gameObject.name}:  {item.Key} is not {typeof(T).Name}");

                return value;
            }

            throw new System.Exception($"{gameObject.name}: {key} not found");
        }

        [ContextMenu("Print Data Log")]
        public void PrintDebugLog()
        {
            string log = "";
            foreach (var item in _variantParam)
            {
                log += $"{item.Key} : {item.Param}";
            }

            Debug.Log($"{log}\n{GetCutsceneObjectData(new())}");
        }
    }
}
