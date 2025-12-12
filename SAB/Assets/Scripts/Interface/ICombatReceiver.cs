/// <summary>
/// 공격을 입력을 받을 수 있음
/// </summary>
public interface ICombatReceiver : IInputReceiver
{
    public void Attack();
}
