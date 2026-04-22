using Newtonsoft.Json;
using UnityEngine;

namespace SAB.Cutscene
{
    public struct SingleMeshData
    {
        public int ID;
        public string TrackName;
        public string MeshPath;

        public float posX, posY, posZ;
        public float RotX, RotY, RotZ, RotW;

        [JsonIgnore]
        public readonly Vector3 Position
        {
            get => new(posX, posY, posZ);
        }

        [JsonIgnore]
        public readonly Quaternion Rotation
        {
            get => new(RotX, RotY, RotZ, RotW);
        }

        public void SetPose(Vector3 pos, Quaternion quaternion)
        {
            posX = pos.x;
            posY = pos.y;
            posZ = pos.z;
            RotX = quaternion.x;
            RotY = quaternion.y;
            RotZ = quaternion.z;
            RotW = quaternion.w;
        }

        public readonly override string ToString()
        {
            return $"ID : {ID}, Name : {TrackName}, Mesh Path : {MeshPath}, Position : {Position}, Rotation : {Rotation} ";
        }
    }
}
