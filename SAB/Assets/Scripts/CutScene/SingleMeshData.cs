using Newtonsoft.Json;
using UnityEngine;

namespace SAB.Cutscene
{
    public class SingleMeshData : IObjectConfig
    {
        public string MeshPath;

        public float PosX, PosY, PosZ;
        public float RotX, RotY, RotZ, RotW;

        [JsonIgnore]
        public Vector3 Position
        {
            get => new(PosX, PosY, PosZ);
        }

        [JsonIgnore]
        public Quaternion Rotation
        {
            get => new(RotX, RotY, RotZ, RotW);
        }

        public void SetPose(Vector3 pos, Quaternion quaternion)
        {
            PosX = pos.x;
            PosY = pos.y;
            PosZ = pos.z;
            RotX = quaternion.x;
            RotY = quaternion.y;
            RotZ = quaternion.z;
            RotW = quaternion.w;
        }

        public override string ToString()
        {
            return $"Mesh Path : {MeshPath}, Position : {Position}, Rotation : {Rotation}";
        }
    }
}
