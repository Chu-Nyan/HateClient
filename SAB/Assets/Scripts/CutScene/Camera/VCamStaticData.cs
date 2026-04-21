using Newtonsoft.Json;
using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamStaticData : IVCamData
    {
        // VCam
        public int ID;
        public float posX, posY, posZ;
        public float RotX, RotY, RotZ, RotW;
        public float POV;

        [JsonIgnore]
        public Vector3 Position
        {
            get => new(posX, posY, posZ);
        }

        [JsonIgnore]
        public Quaternion Rotation
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
    }
}
