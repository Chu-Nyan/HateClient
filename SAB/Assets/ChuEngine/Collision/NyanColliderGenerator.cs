using System;
using Chu.Collision;
using Chu.Utility;
using UnityEngine;

public class NyanColliderGenerator
{
    private readonly IDNumbering _iDNumbering;

    private NyanCollider _newCollider;

    private event Action<NyanCollider> GeneratedEvent;

    public NyanColliderGenerator()
    {
        _iDNumbering = new(0, 64);
    }

    public void RegisterGenerated(Action<NyanCollider> action) 
    {
        GeneratedEvent += action;
    }

    public NyanColliderGenerator GenerateCollider(Transform transform, Shape shape, string comment)
    {
        _newCollider = new NyanCollider(transform, shape, _iDNumbering.GetID(), comment);
        return this;
    }

    public NyanColliderGenerator GenerateCollider(Transform transform, Shape shape)
    {
        GenerateCollider(transform, shape, "Collider");
        return this;
    }

    public NyanCollider GetCollider()
    {
        GeneratedEvent?.Invoke(_newCollider);
        return _newCollider;
    }
}
