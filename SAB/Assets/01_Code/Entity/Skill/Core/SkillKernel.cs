using System;

namespace SAB.Skill
{
    /// <summary>
    /// 스킬의 공용, 개별 정보
    /// </summary>
    public class SkillKernel
    {
        public readonly int ID;
        private SkillData _data;
        private float _remainingCooldown;
        private bool _canUse;

        public bool CanUse
        {
            get => _canUse;
        }

        public SkillData Data
        {
            get => _data;
        }

        public SkillKernel(int id)
        {
            ID = id;
        }

        public void Setup(SkillData data)
        {
            _data = data;
            _remainingCooldown = 0;
        }

        public void TryUse()
        {
            if (CanUse == false)
                return;

            _remainingCooldown = _data.Cooldown;
            _canUse = false;
        }

        public void ReduceCooldown(float time)
        {
            _remainingCooldown = Math.Max(0f, _remainingCooldown - time);
            if (_remainingCooldown <= 0)
                _canUse = true;
        }
    }
}
