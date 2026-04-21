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

        public VCamStaticData GetStaticData()
        {
            VCamStaticData data = new()
            {
                ID = gameObject.GetInstanceID(),
                POV = _vcam.Lens.FieldOfView
            };
            data.SetPose(transform.position, transform.rotation);
            return data;
        }

        public IVCamData GetSerializedData()
        {
            return GetStaticData();
        }
    }
}
