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

    public bool IsDead
    {
        get => _currentStats.HP <= 0;
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

    public void SetBaseData(CharacterBaseStats baseStats, IdleData idle, PatrolData patrol)
    {
        _baseStats = baseStats;
        _idleData = idle;
        _patrolData = patrol;
    }

    public void SetCurrentStats(float hp)
    {
        _currentStats.HP = hp;
    }

    public void ResetStats()
    {
        _currentStats.HP = BaseStats.HP;
    }
}
