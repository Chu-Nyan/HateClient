/// <summary>
/// 유닛 행동 전략과 수신 객체를 제어
/// </summary>
public class UnitBehaviorHandler
{
    private static int _idCounter = 0;

    private readonly int _id;
    private int _receiverID;

    private IUnitBehaviorStrategy _behaviorStrategy;
    private IActionReceiver _actionReceiver;
    private IMovementReceiver _movementReceiver;

    private bool _isActivation;

    public int ID
    {
        get => _id;
    }

    public int ReceiverID
    {
        get => _receiverID;
    }

    public bool IsActivation
    {
        get => _isActivation;
    }

    public UnitBehaviorHandler()
    {
        _id = ++_idCounter;
    }

    public void Update()
    {
        _behaviorStrategy.Update();
    }

    public void SetActive(bool isActivation)
    {
        if (isActivation == _isActivation)
            return;

        _isActivation = isActivation;
        if (isActivation == true)
            _behaviorStrategy.Enable();
        else
            _behaviorStrategy.Disable();
    }

    public void SetBehaviorStrategy(IUnitBehaviorStrategy unitBehavior)
    {
        _behaviorStrategy?.Disable();

        _behaviorStrategy = unitBehavior;
        RefreshBehaviorStrategy();
        if (_isActivation == true)
            _behaviorStrategy.Enable();
    }

    public void SetReceivers<T>(T receiver) where T : IInputReceiver
    {
        SetActionReceiver(receiver as IActionReceiver);
        SetMovementReceiver(receiver as IMovementReceiver);
        _receiverID = receiver.ReceiverID;
    }

    private void SetActionReceiver(IActionReceiver receiver)
    {
        _actionReceiver = receiver;
        RefreshBehaviorStrategy();
    }

    private void SetMovementReceiver(IMovementReceiver receiver)
    {
        _movementReceiver = receiver;
        RefreshBehaviorStrategy();
    }

    private void RefreshBehaviorStrategy()
    {
        _behaviorStrategy.SetMovementReceiver(_movementReceiver);
        _behaviorStrategy.SetActionReceiver(_actionReceiver);
    }
}
