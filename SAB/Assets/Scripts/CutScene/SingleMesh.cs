using SAB.Cutscene;
using System;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace SAB
{
    public class SingleMesh : MonoBehaviour, ICutsceneSerializable
    {
        [SerializeField]
        private MeshFilter _filter;
        [SerializeField]
        private MeshRenderer _renderer;
        [SerializeField]
        private Animator _animator;

        public int ID
        {
            get => gameObject.GetInstanceID();
        }

        public Animator Animator
        {
            get => _animator;
        }

        public void ApplySerializedData(SingleMeshData data)
        {
            transform.position = data.Position;
            transform.rotation = data.Rotation;

            var mesh = AssetManager.LoadAssetSync<Mesh>(data.MeshPath);
            _filter.mesh = mesh;
            SetMesh(mesh);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        private void SetMesh(Mesh mesh)
        {
            _filter.mesh = mesh;
        }

        public SingleMeshData GetData(string trackName)
        {
            int id = gameObject.GetInstanceID();

            SingleMeshData data = new()
            {
                ID = id,
                TrackName = trackName,
                MeshPath = GetAddressablePath(GetComponent<MeshFilter>().sharedMesh)
            };
            data.SetPose(transform.position, transform.rotation);

            return data;
        }

        private string GetAddressablePath(Mesh mesh)
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(mesh);
            string guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings.FindAssetEntry(guid);

            if (entry != null)
                return entry.address;
            else
                throw new System.Exception($"{mesh.name}, No Addressable Asset");
        }

        [ContextMenu("Print Data Log")]
        public void PrintDebugLog()
        {
            SingleMeshData data = GetData("Debug");
            Debug.Log(data.ToString());
        }
    }
}
