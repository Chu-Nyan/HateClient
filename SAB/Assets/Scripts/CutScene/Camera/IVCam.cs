using Unity.Cinemachine;

namespace SAB.Cutscene
{
    public interface IVCam : ICutsceneObject
    {
        public CinemachineCamera CinemachineCamera { get; }
    }
}
