using SAB.GameSystem;
using Unity.Cinemachine;

namespace SAB.Cutscene
{
    public interface IVCam : ICutsceneObject
    {
        public VCamType Type { get; }

        public CinemachineCamera CinemachineCamera { get; }
    }
}
