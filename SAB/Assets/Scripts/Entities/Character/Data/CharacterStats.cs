public class CharacterStats : IMovementAIDataView
{
    private CharacterBaseStats _baseStats;

    private IdleData _idleData;
    private PatrolData _patrolData;

    public CharacterBaseStats BaseStats
    {
        get => _baseStats;
    }

    public IdleData IdleData
    {
        get => _idleData;
    }

    public PatrolData PatrolData
    {
        get => _patrolData;
    }

    public void SetData(CharacterBaseStats baseStats)
    {
        _baseStats = baseStats;
    }

    public void SetMovementAIData(IdleData idle, PatrolData patrol)
    {
        _idleData = idle;
        _patrolData = patrol;
    }
}
