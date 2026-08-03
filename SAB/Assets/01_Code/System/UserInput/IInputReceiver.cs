using SAB.EntityAgent;

/// <summary>
/// Player, AI의 제어에 필요한 정보를 제공
/// </summary>
public interface IInputReceiver
{
    public int ReceiverID { get; }
    public BrainType BrainType { get; }

    public void OnOwnerChanged(BrainType isPlayer);
}
