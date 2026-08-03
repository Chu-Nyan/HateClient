using Unity.Cinemachine;

namespace SAB.Cutscene
{
    public interface IVCam : IOnlyCutscene
    {
        public CinemachineCamera CinemachineCamera { get; }
    }
}
