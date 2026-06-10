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

        public CutsceneObjectType CutsceneType
        {
            get => CutsceneObjectType.SingleMesh;
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

        public SingleMeshData GetSingleMeshData()
        {
            SingleMeshData data = new()
            {
                MeshPath = Chu.Utility.UnityHelper.Utility.GetAddressablePath(_filter.sharedMesh)
            };
            data.SetPose(transform.position, transform.rotation);

            return data;
        }

        public IObjectConfig GetCutsceneConfig()
        {
            return GetSingleMeshData();
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
