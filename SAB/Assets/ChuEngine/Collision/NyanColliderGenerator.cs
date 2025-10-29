using System;
using Chu.Collision;
using Chu.Utility;
using UnityEngine;

public class NyanColliderGenerator
{
    private readonly IDNumbering _iDNumbering;

    private CollisionSystem _system;
    private NyanCollider _newCollider;
    private bool _initialized;

    private event Action<NyanCollider> CommonEnabledChangedHandler;

    public NyanColliderGenerator()
    {
        _iDNumbering = new(0, 64);
    }

    public void Init(CollisionSystem sys)
    {
        if (_initialized == true)
            throw new Exception("초기화 중복 호출");

        _system = sys;
        _initialized = true;
    }

    public NyanColliderGenerator GenerateCollider(ICollisionProvider provider, Transform transform, Shape shape, string comment)
    {
        if (_initialized == false)
            throw new Exception("초기화 되지 않음");

        _newCollider = new NyanCollider(transform, shape, _iDNumbering.GetID(), comment);
        _newCollider.RegisterEnabled(CommonEnabledChangedHandler);
        SetProviderAndRegisterCollisionSystem(provider);
        return this;
    }

    public NyanColliderGenerator GenerateCollider(ICollisionProvider provider, Transform transform, Shape shape)
    {
        GenerateCollider(provider, transform, shape, "Collider");
        return this;
    }

    private NyanColliderGenerator SetProviderAndRegisterCollisionSystem(ICollisionProvider provider)
    {
        provider.Collider = _newCollider;
        _system.RegisterEntity(provider);
        return this;
    }

    public NyanCollider GetCollider()
    {
        return _newCollider;
    }
}
