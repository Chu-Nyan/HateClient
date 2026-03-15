using Chu;
using Chu.Collision;
using Chu.Collision.Layer;
using UnityEngine;

public class CharacterBody : INyanCollisionProvider, IDefendable
{
    private const NyanLayer _layer = NyanLayer.Unit;

    private IDefendable _hitReceiver;
    private Transform _transform;
    private NyanCollider _collider;

    public Transform transform
    {
        get => _transform;
    }

    public NyanCollider Collider
    {
        get => _collider;
    }

    public CharacterBody(int instigatorID, IDefendable hitReceiver, Transform transform, IShape body)
    {
        _transform = transform;
        _hitReceiver = hitReceiver;
        var mask = new NyanLayerMask(NyanLayer.Projectile, NyanLayer.UnitSensor);

        _collider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
            .GenerateCollider(this, "캐릭터 바디")
            .SetShape(body)
            .SetLayer(_layer, mask)
            .SetInstigatorID(instigatorID)
            .GetCollider(true);
    }

    public void OnNyanCollisionEnter(INyanCollisionProvider provider)
    {
    }

    public void OnNyanCollisionExit(INyanCollisionProvider provider)
    {
    }

    public void Defend(AttackContext context, HitResult hit)
    {
        _hitReceiver.Defend(context, hit);
    }
}
