/// <summary>
/// 상호작용, 공격 등 행동을 수신
/// </summary>
public interface IActionReceiver : IInputReceiver
{
    public void Attack();
    public void Interact();
}
