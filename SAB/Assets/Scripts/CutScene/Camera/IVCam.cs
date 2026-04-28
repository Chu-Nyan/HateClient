using Unity.Cinemachine;

namespace SAB.Cutscene
{
    public interface IVCam
    {
        public CinemachineCamera CinemachineCamera { get; }
        public void SetActive(bool value);
    }
}
