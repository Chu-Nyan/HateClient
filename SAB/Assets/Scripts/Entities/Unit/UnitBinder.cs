using SAB.AI;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnitBehaviorHandler 통합 관리
/// </summary>
public class UnitBinder : MonoBehaviour
{
    private const int _playerHandlerID = 0;
    private UnitBehaviorHandler[] _handlerByHandlerId;
    private Dictionary<int, UnitBehaviorHandler> _activationHandlerByReceiverId;
    private Queue<UnitBehaviorHandler> _idleHandlers;

    private void Awake()
    {
        _handlerByHandlerId = new UnitBehaviorHandler[200];
        _activationHandlerByReceiverId = new();
        _idleHandlers = new();
        GenerateHanlder(new PlayerInputBehaviorStrategy());
    }

    private void Update()
    {
        foreach (var item in _activationHandlerByReceiverId)
        {
            item.Value.Tick();
        }
    }

    public void BindReceiver(IInputReceiver receiver, UnitController.Oner oner)
    {
        UnitBehaviorHandler handler;
        if (oner == UnitController.Oner.Player)
        {
            handler = _handlerByHandlerId[_playerHandlerID];
        }
        else
        {
            if (_idleHandlers.Count == 0)
            {
                UnitBehaviorHandler aiHandler = GenerateHanlder(AIGenerator.Instance.TempGenerate());
                _idleHandlers.Enqueue(aiHandler);
            }
            handler = _idleHandlers.Dequeue();
        }

        handler.SetReceivers(receiver);
        handler.SetActive(true);
        _activationHandlerByReceiverId[receiver.ReceiverID] = handler;
    }

    public void UnbindReceiver(IInputReceiver receiver)
    {
        _activationHandlerByReceiverId[receiver.ReceiverID].SetActive(false);
        _activationHandlerByReceiverId.Remove(receiver.ReceiverID);
    }

    private UnitBehaviorHandler GenerateHanlder(IUnitBehaviorStrategy strategy)
    {
        // TODO : PlayerHandler를 제외하고 돌려쓸 수 있게 변경
        var handler = new UnitBehaviorHandler();
        handler.SetBehaviorStrategy(strategy);
        _handlerByHandlerId[handler.ID] = handler;
        _idleHandlers.Enqueue(handler);
        return handler;
    }
}
