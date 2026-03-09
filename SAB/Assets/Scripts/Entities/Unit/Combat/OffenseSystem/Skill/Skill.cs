using System;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬의 공용, 개별 정보
    /// </summary>
    public class Skill
    {
        public readonly int ID;
        private SkillData _data;
        private float _remainingCooldown;

        public float RemainingCooldown
        {
            get => _remainingCooldown;
        }

        public bool CanUse
        {
            get => _remainingCooldown <= 0;
        }

        public SkillData Data
        {
            get => _data;
        }

        public Skill(int id)
        {
            ID = id;
        }

        public void Setup(SkillData data)
        {
            _data = data;
            _remainingCooldown = 0;
        }

        public void Use()
        {
            _remainingCooldown = _data.Cooldown;
        }

        public void ReduceCooldown(float time)
        {
            _remainingCooldown = Math.Max(0f, _remainingCooldown - time);
        }
    }
}
