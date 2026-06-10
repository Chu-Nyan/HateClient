using Unity.Cinemachine;
using UnityEngine;

namespace SAB.Cutscene
{
    public class VCamStatic : MonoBehaviour, IVCam
    {
        [SerializeField]
        private CinemachineCamera _vcam;

        public CutsceneObjectType CutsceneType
        {
            get => CutsceneObjectType.VCamStatic;
        }

        public CinemachineCamera CinemachineCamera
        {
            get => _vcam;
        }

        public void ApplySerializedData(VCamStaticData data)
        {
            _vcam.transform.SetPositionAndRotation(data.Position, data.Rotation);
            _vcam.Lens.FieldOfView = data.POV;
        }

        public void SetCutsceneData(IObjectConfig data)
        {
            if (data is not VCamStaticData staticData)
                throw new System.Exception(data.GetType().ToString());

            ApplySerializedData(staticData);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public VCamStaticData GetVCamStaticConfig()
        {
            VCamStaticData data = new()
            {
                POV = _vcam.Lens.FieldOfView
            };
            data.SetPose(transform.position, transform.rotation);

            return data;
        }

        public IObjectConfig GetCutsceneConfig()
        {
            return GetVCamStaticConfig();
        }
    }
}
