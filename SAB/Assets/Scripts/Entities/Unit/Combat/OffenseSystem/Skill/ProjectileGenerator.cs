using Chu.Collision;
using Chu.Utility;
using UnityEngine;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 원거리 공격(발사체) 생성
    /// </summary>
    public class ProjectileGenerator : Singleton<ProjectileGenerator>
    {
        private SkillProjectile _new;

        public ProjectileGenerator Set(IShape shape, int instigator, AttackContext context, Vector3 start, Vector3 dir)
        {
            _new = AssetManager.GenerateLoadAssetSync<SkillProjectile>("Projectile");
            _new.Refresh(shape, instigator, context);
            _new.SetTarget(start, dir);
            _new.SetActive(true);
            return this;
        }

        //public ProjectileGenerator Set(int instigator, AttackContext context, Vector3 start, Vector3 dir)
        //{
        //    _new = AssetManager.GenerateLoadAssetSync<SkillProjectile>("Projectile");
        //    _new.Refresh(shape, instigator, context);
        //    _new.SetTarget(start, dir);
        //    _new.SetActive(true);
        //    return this;
        //}

        public SkillProjectile Get()
        {
            return _new;
        }
    }
}
