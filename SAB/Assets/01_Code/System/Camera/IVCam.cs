using SAB.GameSystem;
using Unity.Cinemachine;

namespace SAB.Cutscene
{
    public interface IVCam : IEntity
    {
        public VCamType Type { get; }

        public CinemachineCamera CinemachineCamera { get; }
    }
}
