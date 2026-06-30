using UnityEngine;

namespace SAB.Cutscene
{
    public class SingleMeshData : ICutscenePreset
    {
        public string MeshPath;

        public Vector3 Position;
        public Quaternion Rotation;

        public SingleMeshData(string meshPath, Vector3 position, Quaternion rotation)
        {
            MeshPath = meshPath;
            Position = position;
            Rotation = rotation;
        }

        public override string ToString()
        {
            return $"Mesh Path : {MeshPath}, Position : {Position}, Rotation : {Rotation}";
        }
    }
}
