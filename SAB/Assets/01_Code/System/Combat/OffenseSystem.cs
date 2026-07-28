using Chu.Collision;
using Chu.Utility;
using SAB.Skill;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.GameSystem
{
    public class OffenseSystem
    {
        public const int BasicAttackIndex = 0;

        private readonly int _instigatorID;
        private readonly Transform _origin;
        private readonly List<SkillKernel> _skillList;
        private readonly List<int> _used;
        private FactionType _faction;

        public List<SkillKernel> SkillList
        {
            get => _skillList;
        }

        public OffenseSystem(int instigatorID, Transform origin)
        {
            _instigatorID = instigatorID;
            _skillList = new List<SkillKernel>();
            _used = new List<int>();
            _origin = origin;
        }

        public void SetFaction(FactionType faction)
        {
            _faction = faction;
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

        public void AddSkill(SkillKernel skill)
        {
            _skillList.Add(skill);
        }

        private void Attack(float damage, int index, Vector3 targetPoint)
        {
            var context = new AttackContext(SkillList[index].Data, _faction, new NyanLayerMask(Const.Layer_Unit), damage);
            var dir = targetPoint - _origin.position;
            dir.y = 0f;
            dir.Normalize();
            SkillFactory.Instance.CreateObject(_instigatorID, context, _origin.position, dir);
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
