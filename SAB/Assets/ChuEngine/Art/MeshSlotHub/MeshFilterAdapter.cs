using UnityEngine;

namespace Chu.Art
{
    public class MeshFilterAdapter : IMeshAdapter
    {
        private readonly MeshFilter _filter;

        public MeshFilterAdapter(MeshFilter meshFilter)
        {
            _filter = meshFilter;
        }

        public void SetMesh(Mesh mesh)
        {
            _filter.sharedMesh = mesh;
        }
    }
}
