using Chu;
using Chu.Collision;
using Chu.Collision.Layer;
using SAB.Unit.Combat;
using System;
using UnityEngine;

public class CharacterBody : INyanCollisionProvider
{
    private const NyanLayer _layer = NyanLayer.Unit;

    private Transform _transform;
    private NyanCollider _collider;
    private event Action<AttackContext> _onHit;

    public Transform transform
    {
        get => _transform;
    }

    public NyanCollider Collider
    {
        get => _collider;
    }

    public CharacterBody(int instigatorID, Transform transform, IShape body)
    {
        _transform = transform;
        var mask = new NyanLayerMask(NyanLayer.Projectile, NyanLayer.UnitSensor);

        _collider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
            .GenerateCollider(this, body, "캐릭터 바디")
            .SetLayer(_layer, mask)
            .SetInstigatorID(instigatorID)
            .GetCollider();

        _collider.SetActive(true);
    }

    public void OnNyanCollisionEnter(INyanCollisionProvider provider)
    {
        if (provider.Collider.Layer == NyanLayer.Projectile)
        {
            var handler = provider as IAttackContextProvider;
            AttackContext excutor = handler.Context;
            _onHit?.Invoke(excutor);
        }
    }

    public void OnNyanCollisionExit(INyanCollisionProvider provider)
    {
    }

    public void RegisterOnSkillHit(Action<AttackContext> action)
    {
        _onHit += action;
    }

    public void RefreshTransform()
    {
        _collider.RefreshTransform();
    }
}
