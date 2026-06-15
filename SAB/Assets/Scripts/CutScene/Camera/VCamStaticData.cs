using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamStaticData : IObjectConfig
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float POV;

        public void SetPose(Vector3 pos, Quaternion quaternion)
        {
            Position = pos;
            Rotation = quaternion;
        }
    }
}
