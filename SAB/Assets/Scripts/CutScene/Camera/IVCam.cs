using Unity.Cinemachine;

namespace SAB.Cutscene
{
    public interface IVCam
    {
        public CinemachineCamera CinemachineCamera { get; }
        public IVCamData GetSerializedData();
        public void SetActive(bool value);
    }
}
