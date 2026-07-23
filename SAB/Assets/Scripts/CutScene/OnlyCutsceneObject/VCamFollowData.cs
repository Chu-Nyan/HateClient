using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamFollowData : ICutscenePreset
    {
        // VCam
        public Quaternion Rotation;
        public float POV;

        // Follow
        public int TargetID;
        public Vector3 FollowOffset;

        public VCamFollowData(Quaternion rotation, float pov, int targetID, Vector3 followOffset)
        {
            Rotation = rotation;
            POV = pov;
            TargetID = targetID;
            FollowOffset = followOffset;
        }
    }
}
