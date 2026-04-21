using Newtonsoft.Json;
using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamFollowData : IVCamData
    {
        // VCam
        public int ID;
        public float RotX, RotY, RotZ, RotW;
        public float POV;

        // Follow
        public int TargetID;
        public float OffsetX, OffsetY, OffsetZ;

        [JsonIgnore]
        public Quaternion Rotation
        {
            get => new (RotX, RotY, RotZ, RotW);
        }

        [JsonIgnore]
        public Vector3 FollowOffset
        {
            get => new(OffsetX, OffsetY, OffsetZ);
        }

        public void SetPose(Quaternion quaternion)
        {
            RotX = quaternion.x;
            RotY = quaternion.y;
            RotZ = quaternion.z;
            RotW = quaternion.w;
        }

        public void SetFollow(Vector3 followOffset)
        {
            OffsetX = followOffset.x;
            OffsetY = followOffset.y;
            OffsetZ = followOffset.z;
        }
    }
}
