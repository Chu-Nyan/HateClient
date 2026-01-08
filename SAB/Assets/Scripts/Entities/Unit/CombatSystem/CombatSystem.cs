using Chu.Collision;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Unit.Combat
{
    public class CombatSystem
    {
        public const int BasicAttackIndex = 0;
        private static readonly HashSet<int> _toRemove = new();
        private List<Skill> _skillList;
        private HashSet<int> _used;

        public List<Skill> SkillList
        {
            get => _skillList;
        }

        public CombatSystem()
        {
            _skillList = new List<Skill>();
            _used = new HashSet<int>();
        }

        public void AddSkill(Skill skill)
        {
            _skillList.Add(skill);
        }

        public void Update()
        {
            if (_used.Count <= 0)
                return;

            var time = Time.deltaTime;
            foreach (var index in _used)
            {
                var skill = _skillList[index];
                skill.ReduceCooldown(time);

                if (skill.CanUse == true)
                    _toRemove.Add(index);
            }

            if (_toRemove.Count > 0)
            {
                foreach (var index in _toRemove)
                {
                    _used.Remove(index);
                }
                _toRemove.Clear();
            }
        }

        public void Attack(int instigatorID, int index, Vector3 start, Vector3 targetPoint)
        {
            if (_used.Contains(index) == true)
                return;

            _used.Add(index);
            Skill skill = SkillList[index];
            skill.Use();
            var context = new AttackContext(skill.Data, 10); // 10 -> 객체의 공격력 추가

            // TODO : 스킬에 맞는 shape 발사

            var rect = new CircleShape(0.5f);
            var dir = targetPoint - start;
            dir.y = 0f;
            Debug.DrawRay(start, dir * 50f, Color.red, 4f);
            // 원거리 공격
            ProjectileGenerator.Instance.Set(rect, instigatorID, context, start, dir);
            Debug.Log($"{index}번 스킬, {targetPoint} 공격");
        }
    }
}
