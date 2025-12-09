using Chu.Utility;
using System;

public class CharacterGenerator
{
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
        _new.Init(_numbering.GetID());
        _canRelease = true;
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
