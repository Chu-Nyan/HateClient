using System;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬의 공용, 개별 정보
    /// </summary>
    public class Skill
    {
        private static int _idCounter;

        public readonly int ID;
        private SkillData _data;
        private float _remainingCooldown;
        private bool _canUse;

        public float RemainingCooldown
        {
            get => _remainingCooldown;
        }

        public bool CanUse
        {
            get => _canUse;
        }

        public SkillData Data
        {
            get => _data;
        }

        public Skill()
        {
            ID = ++_idCounter;
        }

        public void Init(SkillData data)
        {
            _data = data;
            _remainingCooldown = 0;
            _canUse = true;
        }

        public void Use()
        {
            _remainingCooldown = _data.Cooldown;
        }

        public void ReduceCooldown(float time)
        {
            _remainingCooldown = Math.Max(0f, _remainingCooldown - time);
            _canUse = _remainingCooldown <= 0;
        }
    }
}
