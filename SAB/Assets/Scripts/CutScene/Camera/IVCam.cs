using Unity.Cinemachine;

namespace SAB.Cutscene
{
    public interface IVCam
    {
        public IVCamData GetSerializedData();
        public CinemachineCamera CinemachineCamera { get; }
    }
}
