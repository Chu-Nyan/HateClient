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
        public const string _aiType = "AIType";
        public const string _bindingObject = "Binding Object";
        public const string _bindingSlot = "Binding Slot";

        public CutsceneObjectType Type;
        public BindingSource BindingSource;
        public bool IsPersistent;

        [SerializeField, HideInInspector]
        private CutsceneObjectType _prevType = CutsceneObjectType.SingleMesh;
        [SerializeField, HideInInspector]
        private BindingSource _prevSource = BindingSource.SceneObject;

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
            if (_prevSource == BindingSource && _prevType == Type)
                return;
            if (VariantParam == null)
                VariantParam = new();

            VariantParam.Clear();

            if (BindingSource == BindingSource.SceneObject)
            {
                TryAddParam<GameObject>(_bindingObject, null);
            }
            else if (BindingSource == BindingSource.Slot)
            {
                TryAddParam(_bindingSlot, UniqueEntityType.Player);
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

        public T GetVariantParam<T>(string key)
        {
            foreach (var item in VariantParam)
            {
                if (item.Key != key)
                    continue;

                //if (item.Param.GetValue() is not T value)
                throw new System.Exception($"{gameObject.name}:  {item.Key} is not {typeof(T).Name}");

                return value;
            }

            throw new Exception($"{gameObject.name}: {key} not found");
        }

        private void TryAddParam<T>(string key, T value)
        {
            if (Contains(key) == true)
                return;

        }

        private bool Contains(string key)
        {
            foreach (var item in VariantParam)
            {
                if (item.Key == key)
                {
                    return true;
                }
            }

            return false;
        }

        [ContextMenu("Print Data Log")]
        public void PrintDebugLog()
        {
            string log = "";
            foreach (var item in VariantParam)
            {
                log += $"{item.Key} : {item.Param}";
            }

            Debug.Log($"{log}\n{GetCutsceneObjectData(new())}");
        }
    }
}
