using Unity.Cinemachine;
using UnityEngine;

namespace SAB.GameSystem
{
    public class TopViewCamera
    {
        private static readonly Vector3 _defalutFollowOffset = new(0, 6, -7.5f);

        private CinemachineCamera _camera;
        private CinemachineFollow _cinemachineFollow;
        private Transform _cameraArm;

        public void InitCamera(GameObject cameraObj)
        {
            _camera = cameraObj.GetComponent<CinemachineCamera>();
            _cinemachineFollow = cameraObj.GetComponent<CinemachineFollow>();
            _cameraArm = new GameObject("CameraArm").transform;

            _cinemachineFollow.FollowOffset = _defalutFollowOffset;
            SetFollowTarget(_cameraArm);
            SetLookAtTarget(_cameraArm);
        }

        public void StickCameraArm(Transform transform)
        {
            _cameraArm.parent = transform;
            _cameraArm.transform.localPosition = Vector3.zero;
            _camera.PreviousStateIsValid = false;
        }

        public void SetFollowTarget(Transform transform)
        {
            _camera.Follow = transform;
        }

        public void SetLookAtTarget(Transform transform)
        {
            _camera.LookAt = transform;
        }
    }
}
