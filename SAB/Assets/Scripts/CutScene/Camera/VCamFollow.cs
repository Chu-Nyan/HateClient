using Unity.Cinemachine;
using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamFollow : MonoBehaviour, IVCam
    {
        [SerializeField]
        private CinemachineCamera _vcam;
        [SerializeField]
        private CinemachineFollow _follow;

        public CinemachineCamera CinemachineCamera
        {
            get => _vcam;
        }

        public void ApplySerializedData(VCamFollowData data)
        {
            transform.rotation = data.Rotation;
            _vcam.Lens.FieldOfView = data.POV;
            _follow.FollowOffset = data.FollowOffset;
        }

        public VCamFollowData GetFollowData()
        {
            VCamFollowData data = new()
            {
                ID = gameObject.GetInstanceID(),
                POV = _vcam.Lens.FieldOfView,
                TargetID = _vcam.Follow.GetInstanceID(),
            };
            data.SetPose(transform.rotation);
            data.SetFollow(_follow.FollowOffset);
            return data;
        }

        public IVCamData GetSerializedData()
        {
            return GetFollowData();
        }
    }
}
