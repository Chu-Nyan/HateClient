using Chu.Data;
using Chu.Utility.Unity;
using SAB.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneObject : MonoBehaviour
    {
        public const string _meshFilter = "MeshFilter";
        public const string _vcam = "VCam";
        public const string _vcamFollow = "VCamFollow";
        public const string _npcID = "NPCID";

        [SerializeField]
        private CutsceneObjectType _type;
        [SerializeField, HideInInspector]
        private CutsceneObjectType _prevType = CutsceneObjectType.SingleMesh;
        [SerializeField]
        private List<VariantParamPair> _variantParam;

        public CutsceneObjectType Type
        {
            get => _type;
        }

        public int ID
        {
            get => gameObject.name.GetHashCode();
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
            if (_type == _prevType)
                return;

            _prevType = _type;
            _variantParam = new();

            if (_type == CutsceneObjectType.SingleMesh)
            {
                _variantParam.Add(new VariantParamPair(_meshFilter, new VariantParam<MeshFilter>()));
            }
            else if (_type == CutsceneObjectType.VCamFollow)
            {
                _variantParam.Add(new VariantParamPair(_vcam, new VariantParam<Component>()));
                _variantParam.Add(new VariantParamPair(_vcamFollow, new VariantParam<Component>()));
            }
            else if (_type == CutsceneObjectType.VCamStatic)
            {
                _variantParam.Add(new VariantParamPair(_vcam, new VariantParam<Component>()));
            }
            else if (_type == CutsceneObjectType.Character)
            {
                _variantParam.Add(new VariantParamPair(_npcID, new VariantParam<int>()));
            }
        }

        public IObjectConfig GetCutsceneObjectData()
        {
            if (_type == CutsceneObjectType.SingleMesh)
            {
                var singlemesh = GetComponent<SingleMesh>();
                string path = Utility.GetAddressablePath(singlemesh.MeshFilter.sharedMesh);
                return new SingleMeshData(path, transform.position, transform.rotation);
            }
            else if (_type == CutsceneObjectType.VCamFollow)
            {
                var vcam = GetComponent<VCamFollow>();
                var pov = vcam.CinemachineCamera.Lens.FieldOfView;
                return new VCamFollowData(transform.rotation, pov, vcam.TargetID, vcam.CinemachineFollow.FollowOffset);
            }
            else if (_type == CutsceneObjectType.VCamStatic)
            {
                var vcam = GetComponent<VCamStatic>();
                return new VCamStaticData(transform.position, transform.rotation, vcam.CinemachineCamera.Lens.FieldOfView);
            }
            else if (_type == CutsceneObjectType.Character)
            {
                // id 수정 필요
                //var acter = GetComponent<Character>();
                var id = GetVariantParam<int>(_npcID);
                return new SpawnRequest(id, transform.position, transform.rotation);
            }

            throw new System.Exception($"{gameObject.name}: is not {_type}");
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

            Debug.Log($"{log}\n{GetCutsceneObjectData()}");
        }
    }
}
