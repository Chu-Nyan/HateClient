using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamFollowData : IObjectConfig
    {
        // VCam
        public Quaternion Rotation;
        public float POV;

        // Follow
        public int TargetID;
        public Vector3 FollowOffset;

        public void SetPose(Quaternion rotation)
        {
            Rotation = rotation;
        }

        public void SetFollow(Vector3 followOffset)
        {
            FollowOffset = followOffset;
        }
    }
}
