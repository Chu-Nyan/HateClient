using Chu.Utility;
using SAB.DataManger;
using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : Singleton<CharacterGenerator>
{
    private readonly Dictionary<int, BaseStats> _unitDatas;

    private IDNumbering _numbering;
    private Character _new;
    private bool _canRelease;

    public CharacterGenerator(DataBase db)
    {
        _unitDatas = db.CharacterRepo.CharacterBaseData;
        _numbering = new IDNumbering();
    }

    public CharacterGenerator Ready(Vector3 respawn)
    {
        _new = AssetManager.GenerateLoadAssetSync<Character>(Const.Asset_Character);
        _new.transform.position = respawn;
        _new.Init(_numbering.GetID());
        _canRelease = true;
        return this;
    }

    public CharacterGenerator SetData(int id)
    {
        _new.SetupStats(_unitDatas[id]);

        return this;
    }

    public CharacterGenerator SetCustomizing(CustomizingData data)
    {
        _new.SetCustomizing(CustomizingPart.Hair, data.Hair);
        _new.SetCustomizing(CustomizingPart.Mouth, data.Mouth);
        _new.SetCustomizing(CustomizingPart.Eyebrow, data.Eyebrow);
        _new.SetCustomizing(CustomizingPart.Eye, data.Eye);
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
