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

        public CutsceneObjectType CutsceneType
        {
            get => CutsceneObjectType.VCamFollow;
        }

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

        public void SetCutsceneData(IObjectConfig data)
        {
            if (data is not VCamFollowData followData)
                throw new System.Exception(data.GetType().ToString());

            ApplySerializedData(followData);
        }

        public void SetFollow(Transform obj)
        {
            _vcam.Target.TrackingTarget = obj;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public VCamFollowData GetVCamFollowData()
        {
            VCamFollowData data = new()
            {
                POV = _vcam.Lens.FieldOfView,
                TargetID = _vcam.Follow.gameObject.name.GetHashCode(),
            };

            data.SetPose(transform.rotation);
            data.SetFollow(_follow.FollowOffset);

            return data;
        }

        public IObjectConfig GetCutsceneConfig()
        {
            return GetVCamFollowData();
        }
    }
}
