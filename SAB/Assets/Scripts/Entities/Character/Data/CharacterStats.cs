using SAB.Unit;

namespace SAB.Unit
{
    public enum StatType
    {
        HP, ATK, PDEF, MDEF, SPD
    }

    public class CharacterStats : IHasStats
    {
        private BaseStats _baseStats;
        private readonly ModifierStat[] _currentStats;
        private readonly float[] _finalStats;

        public BaseStats BaseStats
        {
            get => _baseStats;
        }

        public bool IsDead
        {
            get => _finalStats[(int)StatType.HP] <= 0;
        }

        public float this[StatType type] 
        { 
            get => _finalStats[(int)type]; 
        }

        public CharacterStats()
        {
            _currentStats = new ModifierStat[5];
            _finalStats = new float[5];
        }

        public void SetBaseData(BaseStats baseStats)
        {
            _baseStats = baseStats;
            for (int i = 0; i < _baseStats.Stats.Length; i++)
            {
                _finalStats[i] = baseStats.Stats[i];
            }
        }

        public void AddHP(float value)
        {
            _finalStats[(int)StatType.HP] += value;
        }

        public float GetDamage()
        {
            return _finalStats[(int)StatType.ATK];
        }
    }
}

public class StateHanlder
{
    private StateContext _stateData;

    public StateContext StateData
    {
        get => _stateData;
    }

    public StateHanlder()
    {
        _stateData = new();
    }
}

public class StateContext
{
    private AniParamator _playAnimationClip;

    private bool _isMoveing;

    private bool _isInCombat;
    private float _remainBattleTime;
    private bool _isAttacking;

    public AniParamator PlayAnimationClip
    {
        get => _playAnimationClip;
        set => _playAnimationClip = value;
    }

    public bool IsMoveing
    {
        get => _isMoveing;
        set => _isMoveing = value;
    }

    public bool IsCombatMode
    {
        get => _isInCombat;
        set => _isInCombat = value;
    }

    public float RemainBattileTime
    {
        get => _remainBattleTime;
        set => _remainBattleTime = value;
    }

    public bool IsAttacking
    {
        get => _isAttacking;
        set => _isAttacking = value;
    }
}
