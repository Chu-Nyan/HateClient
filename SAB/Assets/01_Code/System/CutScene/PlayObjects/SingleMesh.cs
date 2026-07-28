using Chu.Core;
using UnityEngine;

namespace SAB.Cutscene
{
    public class SingleMesh : MonoBehaviour, IOnlyCutscene
    {
        [SerializeField]
        private MeshFilter _filter;
        [SerializeField]
        private Animator _animator;

        public MeshFilter MeshFilter
        {
            get => _filter;
        }

        public Animator Animator
        {
            get => _animator;
        }

        public void ApplySerializedData(SingleMeshData data)
        {
            transform.SetPositionAndRotation(data.Position, data.Rotation);
            var mesh = AssetManager.LoadAssetSync<Mesh>(data.MeshPath);
            _filter.mesh = mesh;
            SetMesh(mesh);
        }

        public void SetCutscenePreset(ICutscenePreset data)
        {
            if (data is not SingleMeshData singleMeshData)
                throw new System.Exception(data.GetType().ToString());

            ApplySerializedData(singleMeshData);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        private void SetMesh(Mesh mesh)
        {
            _filter.mesh = mesh;
        }
    }
}
