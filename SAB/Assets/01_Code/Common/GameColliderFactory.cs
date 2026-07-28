using Chu.Collision;
using Chu.Data;
using UnityEngine;

namespace SAB.GameSystem
{
    public static class GameColliderFactory
    {
        public static NyanCollider CreateCharacterBody(INyanCollisionProvider body, string name, Pose2D pose, int instigatorID)
        {
            var mask = new NyanLayerMask(NyanLayer.Projectile, NyanLayer.UnitSensor);

            NyanCollider collider = NyanColliderFactory.Create(body, new CircleShape(), true, $"{name}: Character Body");
            collider.SetTransform(pose);
            collider.SetLayer(NyanLayer.NPCUnit, mask);
            collider.SetInstigatorID(instigatorID);

            return collider;
        }

        public static NyanCollider CreateCombatModeArea(INyanCollisionProvider provider, int instigatorID)
        {
            var shape = new CircleShape(new CircleRangeData(Vector2.zero, 10));
            var mask = new NyanLayerMask(NyanLayer.PlayerUnit, NyanLayer.NPCUnit);

            NyanCollider collider = NyanColliderFactory.Create(provider, shape, true, "전투 판정");
            collider.SetLayer(NyanLayer.UnitSensor, mask);
            collider.SetInstigatorID(instigatorID);

            return collider;
        }

        public static NyanCollider CreateCutsceneTrigger(INyanCollisionProvider provider, int instigatorID)
        {
            var collider = NyanColliderFactory.Create(provider, new RectShape(), false, $"CutsceneTrigger {instigatorID}");
            collider.SetLayer(NyanLayer.TriggerZone, new NyanLayerMask(NyanLayer.PlayerUnit));
            collider.SetInstigatorID(instigatorID);

            return collider;
        }

        public static NyanCollider CreateSkillProjectile(INyanCollisionProvider provider, int instigatorID)
        {
            var collider = NyanColliderFactory.Create(provider, new RectShape(), false, $"Projectile {instigatorID}");
            collider.SetInstigatorID(instigatorID);

            return collider;
        }
    }
}
