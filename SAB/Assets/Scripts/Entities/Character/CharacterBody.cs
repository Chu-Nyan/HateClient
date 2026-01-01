using Chu;
using Chu.Collision;
using Chu.Collision.Layer;
using UnityEngine;

public class CharacterBody : INyanCollisionProvider
{
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

    public CharacterBody(Transform transform, Shape body)
    {
        _transform = transform;
        _collider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
            .GenerateCollider(this, body, "캐릭터 바디")
            .SetLayer(Character.Layer, new NyanLayerMask())
            .GetCollider();
    }

    public void Init(Shape shape)
    {
        _transform = transform;
        _collider.SetShape(shape);
        _collider.SetActive(true);
    }

    public void OnNyanCollisionEnter(INyanCollisionProvider provider)
    {
        //Debug.Log(provider.Collider.Comment + " 충돌 됨");
    }

    public void OnNyanCollisionExit(INyanCollisionProvider provider)
    {
    }
}
