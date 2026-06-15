using UnityEngine;

namespace SAB.Cutscene
{
    public class SingleMeshData : IObjectConfig
    {
        public string MeshPath;

        public Vector3 Position;
        public Quaternion Rotation;

        public void SetPose(Vector3 pos, Quaternion quaternion)
        {
            Position = pos;
            Rotation = quaternion;
        }

        public override string ToString()
        {
            return $"Mesh Path : {MeshPath}, Position : {Position}, Rotation : {Rotation}";
        }
    }
}
