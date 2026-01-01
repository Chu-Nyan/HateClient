using Chu.Collision;
using Chu.Utility;
using System;
using UnityEngine;

public class CharacterGenerator
{
    private const NyanLayer _characterLayer = NyanLayer.Unit;
    private readonly IDNumbering _numbering;

    private Character _new;
    private bool _canRelease;

    public CharacterGenerator()
    {
        _numbering = new IDNumbering(100000, 48);
    }

    public CharacterGenerator Ready()
    {
        _new = AssetManager.GenerateLoadAssetSync<Character>(Const.Asset_Character);
        _canRelease = true;
        return this;
    }

    public CharacterGenerator SetData(Vector3 respawn)
    {
        // 임시 데이터 코드
        var patrolData = new PatrolData(respawn, new(-10, 10), new(-10, 10));
        var idleData = new IdleData();
        idleData.WaitTime = 3f;
        //

        _new.Init(_numbering.GetID(), patrolData, idleData);
        return this;
    }

    public Character Release()
    {
        if (_canRelease == false)
            throw new Exception();

        _canRelease = false;

        return _new;
    }

}
