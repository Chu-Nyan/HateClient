public class UnitController
{
    public enum Oner { Player, AI, None }

    private CharactorGenerator _actorGenerator;

    public UnitController()
    {
        _actorGenerator = new();
    }

    public Character GenerateCharacter(Oner oner)
    {
        var unit = _actorGenerator
            .Ready()
            .Release();

        return unit;
    }
}
