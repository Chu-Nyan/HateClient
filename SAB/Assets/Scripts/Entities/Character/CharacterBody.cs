using Chu.Collision;
using UnityEngine;

public class CharacterBody : INyanCollisionProvider, IDefendable
{
    private IDefendable _hitReceiver;
    private NyanCollider _collider;

    public NyanCollider Collider
    {
        get => _collider;
    }

    public FactionType FactionType
    {
        get => _hitReceiver.FactionType;
    }

    public CharacterBody(int instigatorID, Vector2 posXZ, float eulerY, IDefendable hitReceiver)
    {
        _hitReceiver = hitReceiver;
        _collider = GameColliderFactory.CreateCharacterBody(this, instigatorID.ToString(), new(posXZ, eulerY), instigatorID);
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

        NyanLayer layer = isPlayer == true ? NyanLayer.PlayerUnit : NyanLayer.NPCUnit;
        _collider.SetLayer(layer, new NyanLayerMask(maskNumber));
    }
}
