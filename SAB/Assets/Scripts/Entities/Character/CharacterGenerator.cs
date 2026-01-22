using Chu.Utility;
using SAB.MeshSlot;
using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator
{
    private Dictionary<UnitType, BaseStats> _unitDatas;
    private IDNumbering _numbering;
    private Character _new;
    private bool _canRelease;

    public CharacterGenerator()
    {
        _unitDatas = AssetManager.DeserializeJsonSync<Dictionary<UnitType, BaseStats>>(Const.Asset_Data_CharacterData);
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

    public CharacterGenerator SetData(UnitType type)
    {
        _new.SetupStats(_unitDatas[type]);

        return this;
    }

    public CharacterGenerator SetMesh(SlotType type, int number)
    {
        Mesh mesh = AssetManager.LoadAssetSync<Mesh>($"{Const.Asset_Mesh_CharacterBody}{number:D2}");
        _new.SetMesh(type, mesh);
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
