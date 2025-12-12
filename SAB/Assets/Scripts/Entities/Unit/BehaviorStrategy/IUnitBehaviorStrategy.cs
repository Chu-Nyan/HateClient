/// <summary>
/// 행동을 제어할 기술을 정의하는 전략 패턴
/// </summary>
public interface IUnitBehaviorStrategy
{
    public void Enable();
    public void Disable();
    public void SetMovementReceiver(IMovementReceiver receiver);
    public void SetActionReceiver(IActionReceiver receiver);
    public void Update();
}
