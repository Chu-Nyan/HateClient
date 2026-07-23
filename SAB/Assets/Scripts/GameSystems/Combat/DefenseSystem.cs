using Chu.Collision;
using SAB.Skill;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.GameSystem
{
    public class DefenseSystem : INyanCollisionProvider
    {
        private List<SkillSequence> _sequences;
        private IDefendable _hitReceiver;
        private NyanCollider _collider;

        public NyanCollider Collider
        {
            get => _collider;
        }

        public FactionType FactionType
        {
            get => _hitReceiver.FactionType;
        }

        public DefenseSystem(int instigatorID, Vector2 posXZ, float eulerY, IDefendable hitReceiver)
        {
            _sequences = new();
            _hitReceiver = hitReceiver;
            _collider = GameColliderFactory.CreateCharacterBody(this, instigatorID.ToString(), new(posXZ, eulerY), instigatorID);
        }

        public void Attack(AttackContext context, HitResult hit)
        {
            SkillSequence sequence = SkillGenerator.Instance.GenerateSequence(context, hit);
            _sequences.Add(sequence);
        }

        public bool Tick(IHasStats stats)
        {
            for (int i = _sequences.Count - 1; i >= 0; i--)
            {
                if (_sequences[i].TickAndCheck(stats) == false)
                    continue;

                //Debug.Log($"{_sequences[i].Context.SkillData.StringID} 제거됨");
                _sequences.RemoveAt(i);
            }
            return true;
        }

        public void OnPositionChanged(Vector2 pos, float euler)
        {
            _collider.SetTransform(pos, euler);
        }

        public void Defend(AttackContext context, HitResult hit)
        {
            _hitReceiver.Defend(context, hit);
        }

        public void OnOwnerChanged(bool isPlayer)
        {
            int maskNumber = (int)NyanLayer.Projectile | (int)NyanLayer.UnitSensor;
            if (isPlayer == true)
                maskNumber |= Const.Layer_AdditionalPlayerUnit;

            NyanLayer layer = isPlayer == true ? NyanLayer.PlayerUnit : NyanLayer.NPCUnit;
            _collider.SetLayer(layer, new NyanLayerMask(maskNumber));
        }

        public void OnNyanCollisionEnter(INyanCollisionProvider provider)
        {
        }

        public void OnNyanCollisionExit(INyanCollisionProvider provider)
        {
        }
    }
}
