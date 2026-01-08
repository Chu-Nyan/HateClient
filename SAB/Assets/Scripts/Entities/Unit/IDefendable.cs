/// <summary>
/// 공격을 받을 수 있는 객체
/// </summary>
public interface IDefendable
{
    public void Defend(AttackContext context);
}
