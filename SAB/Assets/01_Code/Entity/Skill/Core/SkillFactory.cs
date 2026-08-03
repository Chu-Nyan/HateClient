using Chu.Core;
using Chu.Utility;
using SAB.DataManger;
using SAB.GameSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Skill
{
    public class SkillFactory : Singleton<SkillFactory>
    {
        private readonly Dictionary<int, SkillData> _skillDataByID;
        private readonly FactionTable _factionTable;

        private Transform _root;
        private readonly ObjectPooling<SkillObject> _objectPooling;
        private readonly Dictionary<SkillStepType, Func<ISkillStep>> _generateFuncByProcessType;

        public SkillFactory(SkillRepository db, FactionTable faction) : base()
        {
            _root = new GameObject("Projectile").transform;
            _skillDataByID = db.SkillByID;
            _factionTable = faction;
            _objectPooling = new(() => AssetManager.GenerateLoadAssetSync<SkillObject>("Projectile", _root));
            _generateFuncByProcessType = new()
            {
                { SkillStepType.Instant, () => new InstantSkillStep() },
                { SkillStepType.DoT, () => new DoTSkillStep() },
                { SkillStepType.AoE, () => new AoESkillStep() },
                { SkillStepType.Timer, () => new TimerSkillStep() },
            };
        }

        public SkillKernel CreateKernel(int id)
        {
            var skill = new SkillKernel(IDGenerator.Next());
            skill.Setup(_skillDataByID[id]);
            return skill;
        }

        public SkillObject CreateObject(int instigator, AttackContext context, Vector3 start, Vector3 dir)
        {
            var skill = _objectPooling.Dequeue();
            skill.Setup(instigator, context, _factionTable[context.Faction].HostilityMask);
            skill.SetTarget(start, dir);
            skill.SetActive(true);
            skill.Destroyed += Release;

            return skill;
        }

        public SkillSequence CreateSequence(AttackContext context, HitResult hit)
        {
            var onHitStep = context.SkillData.CollisionLogics[hit.LogicID].OnHitSteps;
            var list = new List<ISkillStep>();
            for (int i = 0; i < onHitStep.Length; i++)
            {
                list.Add(GenerateStep(onHitStep[i], context));
            }

            var sequence = new SkillSequence(context, list);
            return sequence;
        }

        private ISkillStep GenerateStep(IStepData data, AttackContext context)
        {
            var type = (SkillStepType)(data.GetID / 100000);
            ISkillStep step = _generateFuncByProcessType[type]?.Invoke();
            step.Refresh(data, context);

            return step;
        }

        private void Release(SkillObject skill)
        {
            _objectPooling.Enqueue(skill);
        }
    }
}
