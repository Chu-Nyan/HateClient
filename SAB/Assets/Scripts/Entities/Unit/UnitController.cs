using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Unit과 관련된 최상위 컨트롤러
/// </summary>
public class UnitController
{
    public enum Oner { Player, AI, None }

    private CharacterGenerator _actorGenerator;
    private UnitBinder _binder;

    public UnitController(Transform gameObj)
    {
        _actorGenerator = new();
        _binder = gameObj.AddComponent<UnitBinder>();
    }

    public Character GenerateCharacter(Oner oner)
    {
        var unit = _actorGenerator
            .Ready()
            .Release();

        return unit;
    }

    public void ToggleUnitHandler(int receiverId, bool isActivation)
    {
        _binder.ToggleHandlerByReceiverId(receiverId, isActivation);
    }

    public void BindUnitHandler(Character acter, Oner oner)
    {
        _binder.BindReceiver(acter, oner);
    }
}
