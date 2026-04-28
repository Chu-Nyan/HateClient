using SAB.Cutscene;
using UnityEngine;

namespace SAB
{
    public class SingleMesh : MonoBehaviour
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
    }
}
