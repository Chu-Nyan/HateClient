using System;
using UnityEngine;

namespace SAB.Unit
{
    [Serializable]
    public class CharacterStats : IHasStats
    {
        [SerializeField]
        private int _chacterID;
        private BaseStats _baseStats;
        private readonly ModifierStat[] _currentStats;
        private readonly float[] _finalStats;

        public int CharacterID
        {
            get => _chacterID;
        }

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
            _chacterID = baseStats.ID;
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
