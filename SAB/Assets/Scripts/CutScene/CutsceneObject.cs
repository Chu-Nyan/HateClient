using Chu.Data;
using System;
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
            if (TryGetComponent<ICutsceneObject>(out var comp) == false)
                throw new Exception($"{name} was not found 'ICutsceneObject'");

            return comp.GetCutsceneConfig();
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
