using SAB.GameSystem;

namespace SAB.EntityAgent
{
    /// <summary>
    /// 행동을 제어할 기술을 정의하는 전략 패턴
    /// </summary>
    public interface IBrainStrategy
    {
        public BrainType BrainType { get; }

        public void Enable();
        public void Disable();
        public void SetMovementReceiver(IMovementReceiver receiver);
        public void SetCombatReceiver(IOffenseReceiver receiver);
        public void Tick();
    }
}