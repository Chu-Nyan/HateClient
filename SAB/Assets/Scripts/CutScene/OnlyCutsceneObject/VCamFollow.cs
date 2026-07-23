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

        public CinemachineFollow CinemachineFollow
        {
            get => _follow;
        }

        public void ApplySerializedData(VCamFollowData data)
        {
            transform.rotation = data.Rotation;
            _vcam.Lens.FieldOfView = data.POV;
            _follow.FollowOffset = data.FollowOffset;
        }

        public void SetCutscenePreset(ICutscenePreset data)
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
    }
}
