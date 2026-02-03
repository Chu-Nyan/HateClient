using SAB.EntityAgent;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Unit과 관련된 최상위 컨트롤러
/// </summary>
public class UnitController
{
    public enum Oner { Player, AI, None }

    private CharacterGenerator _actorGenerator;
    private AgentController _controller;

    public UnitController(Transform gameObj)
    {
        _actorGenerator = new();
        _controller = gameObj.AddComponent<AgentController>();
    }

    public Character GenerateCharacter(UnitType type, Vector3 respawn, CustomizingData customizingData)
    {
        var unit = _actorGenerator
            .Ready(respawn)
            .SetData(type)
            .SetCustomizing(customizingData)
            .Release();

        unit.RegisterDeactivated(OnCharacterDeactivated);
        return unit;
    }

    public void BindRecevier(IInputReceiver receiver, Oner oner, bool isActivation)
    {
        if (isActivation == true)
            _controller.BindReceiver(receiver, oner);
        else
            _controller.UnbindReceiver(receiver);
    }

    private void OnCharacterDeactivated(IInputReceiver acter)
    {
        _controller.UnbindReceiver(acter);
    }
}
