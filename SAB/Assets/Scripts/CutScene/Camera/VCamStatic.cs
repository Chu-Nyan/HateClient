using Unity.Cinemachine;
using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamStatic : MonoBehaviour, IVCam
    {
        [SerializeField]
        private CinemachineCamera _vcam;

        public CinemachineCamera CinemachineCamera
        {
            get => _vcam; 
        }

        public void ApplySerializedData(VCamStaticData data)
        {
            _vcam.transform.SetPositionAndRotation(data.Position, data.Rotation);
            _vcam.Lens.FieldOfView = data.POV;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
