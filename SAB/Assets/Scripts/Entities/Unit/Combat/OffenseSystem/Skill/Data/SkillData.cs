using Chu.Collision;
using System;
using UnityEngine;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬 공용 정보
    /// </summary>
    public class SkillData
    {
        public SkillID ID;
        public TextID StringID;
        public float CastingTime;
        public float Cooldown;
        public float Cost;
        public float AttackTriggerTiming;
        public string AttachPoint;
        public ShapeParam[] HitBoxes;
        public int[] HitFlowStepIDs;
    }
}
