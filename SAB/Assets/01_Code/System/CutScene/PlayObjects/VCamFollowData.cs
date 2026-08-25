using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamFollowData : ICutscenePreset
    {
        // VCam
        public Quaternion Rotation;
        public float FieldOfView;

        // Follow
        public int TargetID;
        public Vector3 FollowOffset;

        public VCamFollowData(Quaternion rotation, float fov, int targetID, Vector3 followOffset)
        {
            Rotation = rotation;
            FieldOfView = fov;
            TargetID = targetID;
            FollowOffset = followOffset;
        }
    }
}
