using Chu.Utility;
using System;
using System.Collections.Generic;

public class CharacterGenerator
{
    private Dictionary<UnitType, CharacterBaseStats> _unitDatas;
    private IDNumbering _numbering;
    private Character _new;
    private bool _canRelease;

    public CharacterGenerator()
    {
        _unitDatas = AssetManager.DeserializeJsonSync<Dictionary<UnitType, CharacterBaseStats>>(Const.Asset_Data_CharacterData);
        _numbering = new IDNumbering();
    }

    public CharacterGenerator Ready()
    {
        _new = AssetManager.GenerateLoadAssetSync<Character>(Const.Asset_Character);
        _new.Init(_numbering.GetID());
        _canRelease = true;
        return this;
    }

    public CharacterGenerator SetData(UnitType type, IdleData idleData, PatrolData patrolData)
    {
        _new.SetupStats(_unitDatas[type],idleData, patrolData);
        _new.SetCurrentStats(_unitDatas[type].HP);

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
