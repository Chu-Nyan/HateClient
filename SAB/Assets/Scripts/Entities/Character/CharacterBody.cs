using Chu;
using Chu.Collision;
using Chu.Collision.Layer;
using UnityEngine;

public class CharacterBody : INyanCollisionProvider, IDefendable
{
    private const NyanLayer _npcLayer = NyanLayer.NPCUnit;

    private IDefendable _hitReceiver;
    private NyanCollider _collider;

    public NyanCollider Collider
    {
        get => _collider;
    }

    public CharacterBody(int instigatorID, Vector2 pos, float euler, IDefendable hitReceiver)
    {
        _hitReceiver = hitReceiver;
        var mask = new NyanLayerMask(NyanLayer.Projectile, NyanLayer.UnitSensor);
        CircleShape defaultshape = ShapeFactory.Instance.Generate<CircleShape>();
        defaultshape.Setup(new CircleRangeData(Vector2.one, 1));

        _collider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
            .GenerateCollider(this, "캐릭터 바디")
            .SetShape(defaultshape)
            .SetTransform(pos, euler)
            .SetLayer(_npcLayer, mask)
            .SetInstigatorID(instigatorID)
            .GetCollider(true);
    }

    public void OnPositionChanged(Vector2 pos, float euler)
    {
        _collider.SetTransform(pos, euler);
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

    public void OnOwnerChanged(bool isPlayer)
    {
        int maskNumber = (int)NyanLayer.Projectile | (int)NyanLayer.UnitSensor;
        if (isPlayer == true)
            maskNumber |= Const.Layer_AdditionalPlayerUnit;

        NyanLayer layer = isPlayer == true ? NyanLayer.PlayerUnit : _npcLayer;
        _collider.SetLayer(layer, new NyanLayerMask(maskNumber));
    }
}
