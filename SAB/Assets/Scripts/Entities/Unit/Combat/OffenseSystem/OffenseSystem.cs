using Chu.Collision;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Unit.Combat
{
    public class OffenseSystem
    {
        public const int BasicAttackIndex = 0;

        private readonly int _instigatorID;
        private readonly Transform _origin;
        private readonly List<Skill> _skillList;
        private readonly List<int> _used;

        public List<Skill> SkillList
        {
            get => _skillList;
        }

        public OffenseSystem(int instigatorID, Transform origin)
        {
            _skillList = new List<Skill>();
            _used = new List<int>();
            _instigatorID = instigatorID;
            _origin = origin;

        }

        public void Tick()
        {
            if (_used.Count <= 0)
                return;

            var time = Time.deltaTime;
            for (int i = _used.Count - 1; i >= 0; i--)
            {
                var skill = _skillList[i];
                skill.ReduceCooldown(time);
                if (skill.CanUse == true)
                    _used.Remove(i);
            }
        }

        public void AddSkill(Skill skill)
        {
            _skillList.Add(skill);
        }

        private void Attack(float damage, int index, Vector3 targetPoint)
        {
            var context = new AttackContext(SkillList[index].Data, damage);
            var dir = targetPoint - _origin.position;
            dir.y = 0f;
            dir.Normalize();
            Debug.Log(dir);
            SkillObjectFactory.Instance.Set(_instigatorID, context, _origin.position, dir);
            // 계획
            // 스킬의 DB를 읽어보고 원거리, 근거리
        }

        public AniEventData TriggerAttackAndGetAniEventData(float damage, int index, Vector3 targetPoint)
        {
            _used.Add(index);
            SkillList[index].Use();

            var data = new AniEventData()
            {
                Float = damage,
                Int = index,
                Vector3 = targetPoint,
            };
            return data;
        }

        public void AttackWithAnimator(AniEventData data)
        {
            Attack(data.Float, data.Int, data.Vector3);
        }

        public bool IsUsed(int index)
        {
            return _used.Contains(index);
        }
    }
}
