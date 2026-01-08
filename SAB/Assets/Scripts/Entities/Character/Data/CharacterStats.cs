public class CharacterStats : IMovementAIDataView, IHasStats
{
    private CharacterBaseStats _baseStats;
    private CharacterCurrentStats _currentStats; 
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

    public float HP
    {
        get => _currentStats.HP;
        set => _currentStats.HP = value;
    }

    public CharacterStats()
    {
        _currentStats = new();
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
