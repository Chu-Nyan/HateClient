using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnitBehaviorHandler 통합 관리
/// </summary>
public class UnitBinder : MonoBehaviour
{
    private const int _playerHandlerID = 1;
    private Dictionary<int, UnitBehaviorHandler> _handlerByHandlerId;
    private Dictionary<int, UnitBehaviorHandler> _handlerByReceiverId;

    private void Awake()
    {
        _handlerByHandlerId = new();
        _handlerByReceiverId = new();
        GenerateHanlder(new PlayerInputBehaviorStrategy());
    }

    private void Update()
    {
        foreach (var item in _handlerByHandlerId)
        {
            item.Value.Update();
        }
    }

    public void ToggleHandlerByReceiverId(int id, bool isActivation)
    {
        _handlerByReceiverId[id].SetActive(isActivation);
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
            // TODO : NPC 기타 등등 핸들러 가져오기
            handler = _handlerByHandlerId[_playerHandlerID];
        }

        handler.SetReceivers(receiver);
        _handlerByReceiverId[receiver.ReceiverID] = handler;
    }

    private UnitBehaviorHandler GenerateHanlder(IUnitBehaviorStrategy strategy)
    {
        // TODO : PlayerHandler를 제외하고 돌려쓸 수 있게 변경
        var handler = new UnitBehaviorHandler();
        handler.SetBehaviorStrategy(strategy);
        _handlerByHandlerId.Add(handler.ID, handler);

        return handler;
    }
}
