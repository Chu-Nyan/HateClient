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

    public Character GenerateCharacter(UnitType type, Vector3 respawn)
    {
        var unit = _actorGenerator
            .Ready()
            .SetData(type, IdleData.Default, new PatrolData(respawn))
            .Release();

        unit.RegisterDeactivated(OnCharacterDeactivated);
        return unit;
    }

    public void BindRecevier(IInputReceiver receiver, Oner oner, bool isActivation)
    {
        if (isActivation == true)
            _binder.BindReceiver(receiver, oner);
        else
            _binder.UnbindReceiver(receiver);
    }

    private void OnCharacterDeactivated(IInputReceiver acter)
    {
        _binder.UnbindReceiver(acter);
    }
}
