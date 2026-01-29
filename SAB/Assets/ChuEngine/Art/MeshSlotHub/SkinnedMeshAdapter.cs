using UnityEngine;

namespace Chu.Art
{
    public class SkinnedMeshAdapter : IMeshAdapter
    {
        private readonly SkinnedMeshRenderer _skinnedMeshRenderer;

        public SkinnedMeshAdapter(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            _skinnedMeshRenderer = skinnedMeshRenderer;
        }

        public void SetMesh(Mesh mesh)
        {
            _skinnedMeshRenderer.sharedMesh = mesh;
        }
    }
}
