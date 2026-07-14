using Chu.Core;
using Chu.Utility;
using UnityEngine;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 원거리 공격(발사체) 생성
    /// </summary>
    public class SkillObjectFactory : Singleton<SkillObjectFactory>
    {
        private SkillObject _new;
        private FactionTable _factionTable;

        public void Init(FactionTable faction)
        {
            _factionTable = faction;
        }

        public SkillObjectFactory Set(int instigator, AttackContext context, Vector3 start, Vector3 dir)
        {
            _new = AssetManager.GenerateLoadAssetSync<SkillObject>("Projectile");
            _new.Setup(instigator, context, _factionTable[context.Faction].HostilityMask);

            _new.SetTarget(start, dir);
            _new.SetActive(true);
            return this;
        }

        public SkillObject Get()
        {
            return _new;
        }
    }
}
