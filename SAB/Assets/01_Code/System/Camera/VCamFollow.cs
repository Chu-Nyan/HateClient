using SAB.GameSystem;
using System;
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

        private event Action<VCamFollow> Deactivated;

        public VCamType Type
        {
            get => VCamType.Follow;
        }

        public CinemachineCamera CinemachineCamera
        {
            get => _vcam;
        }

        public CinemachineFollow CinemachineFollow
        {
            get => _follow;
        }

        public void Setup(VCamFollowData data)
        {
            transform.rotation = data.Rotation;
            _vcam.Lens.FieldOfView = data.FieldOfView;
            _follow.FollowOffset = data.FollowOffset;
            _follow.TrackerSettings.PositionDamping = data.PositionDamping;
        }

        public void SetThirdPersonTarget(Transform obj)
        {
            SetLockAtTarget(obj);
            SetFollowTarget(obj);
        }

        public void SetLockAtTarget(Transform obj)
        {
            _vcam.LookAt = obj;
            _vcam.PreviousStateIsValid = false;
        }

        public void SetFollowTarget(Transform obj)
        {
            _vcam.Follow = obj;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            Deactivated?.Invoke(this);
            Deactivated = null;
        }

        public void ResgierDeactivated(Action<VCamFollow> callback)
        {
            Deactivated += callback;
        }

        public void UnresgierDeactivated(Action<VCamFollow> callback)
        {
            Deactivated -= callback;
        }
    }
}
