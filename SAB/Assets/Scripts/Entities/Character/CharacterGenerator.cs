using Chu.Core;
using Chu.Utility;
using SAB.DataManger;
using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : Singleton<CharacterGenerator>
{
    private readonly Dictionary<int, BaseStats> _unitDatas;
    private readonly Dictionary<int, CustomizingData> _customizingData;

    private ObjectPooling<Character> _pool;

    private Character _new;
    private bool _canRelease;

    public CharacterGenerator(DataBase db)
    {
        _unitDatas = db.CharacterRepo.CharacterBaseData;
        _customizingData = db.CharacterRepo.CustomizingData;
        _pool = new(() =>
        {
            return AssetManager.GenerateLoadAssetSync<Character>(Const.Asset_Character);
        });
    }

    public CharacterGenerator Ready(Vector3 respawn)
    {
        _new = _pool.Dequeue();
        _new.SetPositionWithNavMash(respawn);
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

    public CharacterGenerator SetCustomizing(int id)
    {
        if (_customizingData.TryGetValue(id, out var data) == true)
            SetCustomizing(data);

        return this;
    }

    public Character Release()
    {
        if (_canRelease == false)
            throw new Exception();

        _canRelease = false;

        return _new;
    }

    public void Enqueue(Character character)
    {
        _pool.Enqueue(character);
    }
}
