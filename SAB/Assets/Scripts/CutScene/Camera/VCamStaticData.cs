using Newtonsoft.Json;
using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamStaticData : IObjectConfig
    {
        // VCam
        public float PosX, PosY, PosZ;
        public float RotX, RotY, RotZ, RotW;
        public float POV;

        [JsonIgnore]
        public CutsceneObjectType Type
        {
            get => CutsceneObjectType.VCamStatic;
        }

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
    }
}
