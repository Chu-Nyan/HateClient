using Chu.Data;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace SAB.Cutscene
{
    public partial class CutsceneObject : MonoBehaviour
    {
        private const string _meshFilter = "MeshFilter";
        private const string _vcam = "VCam";
        private const string _vcamFollow = "VCamFollow";

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
            get => gameObject.GetInstanceID();
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
        }

        public T GetVariantParam<T>(string key)
        {
            foreach (var item in _variantParam)
            {
                if (item.Key != key)
                    continue;
                if (item.Param.GetValue() is not T)
                {
                    Debug.Log(item.Key);
                    Debug.Log(item.Param);
                    Debug.Log(typeof(T).ToString());
                }

                return (T)item.Param.GetValue();
            }

            return default;
        }

        public bool ContainVariantParam(string key)
        {
            foreach (var item in _variantParam)
            {
                if (item.Key != key)
                    continue;

                return true;
            }

            return false;
        }

        public SingleMeshData GetSingleMeshData(string trackName)
        {
            if (_type != CutsceneObjectType.SingleMesh)
            {
                Debug.LogError($"{gameObject.name} is {_type}");
                return default;
            }

            SingleMeshData data = new()
            {
                ID = ID,
                TrackName = trackName,
                MeshPath = Chu.Utility.UnityHelper.Utility.GetAddressablePath(GetVariantParam<MeshFilter>(_meshFilter).sharedMesh)
            };
            data.SetPose(Position, Rotation);

            return data;
        }

        public IVCamData GetVCamData()
        {
            if (_type == CutsceneObjectType.VCamStatic)
            {
                return GetVCamStaticData();
            }
            else if (_type == CutsceneObjectType.VCamFollow)
            {
                return GetVCamFollowData();
            }
            else
            {
                Debug.LogError($"{gameObject.name} is not VCam");
                return null;
            }
        }

        public VCamStaticData GetVCamStaticData()
        {
            if (_type != CutsceneObjectType.VCamStatic)
            {
                Debug.LogError($"{gameObject.name} is {_type}");
                return default;
            }

            var vcam = GetVariantParam<CinemachineCamera>(_vcam);

            VCamStaticData data = new()
            {
                ID = gameObject.GetInstanceID(),
                POV = vcam.Lens.FieldOfView
            };
            data.SetPose(transform.position, transform.rotation);

            return data;
        }

        public VCamFollowData GetVCamFollowData()
        {
            if (_type != CutsceneObjectType.VCamFollow)
            {
                Debug.LogError($"{gameObject.name} is {_type}");
                return default;
            }

            var vcam = GetVariantParam<CinemachineCamera>(_vcam);
            var follow = GetVariantParam<CinemachineFollow>(_vcamFollow);

            VCamFollowData data = new()
            {
                ID = gameObject.GetInstanceID(),
                POV = vcam.Lens.FieldOfView,
                TargetID = vcam.Follow.gameObject.GetInstanceID(),
            };
            data.SetPose(transform.rotation);
            data.SetFollow(follow.FollowOffset);

            return data;
        }

        [ContextMenu("Print Data Log")]
        public void PrintDebugLog()
        {
            if (_type == CutsceneObjectType.SingleMesh)
                Debug.Log(GetSingleMeshData("None").ToString());
            else if (_type == CutsceneObjectType.VCamStatic)
                Debug.Log(GetVCamStaticData());
            else if (_type == CutsceneObjectType.VCamFollow)
                Debug.Log(GetVCamFollowData());
        }
    }
}
