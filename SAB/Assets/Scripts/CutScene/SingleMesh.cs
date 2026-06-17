using Chu.Core;
using SAB.Cutscene;
using UnityEngine;

namespace SAB
{
    public class SingleMesh : MonoBehaviour, ICutsceneObject
    {
        [SerializeField]
        private MeshFilter _filter;
        [SerializeField]
        private MeshRenderer _renderer;
        [SerializeField]
        private Animator _animator;

        public int ID
        {
            get => gameObject.name.GetHashCode();
        }

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

        public void SetCutsceneData(IObjectConfig data)
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
