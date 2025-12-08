using Chu.Utility;
using System;

public class CharactorGenerator
{
    private Character _new;
    private IDNumbering _numbering;

    private bool _canRelease;

    public CharactorGenerator()
    {
        _numbering = new IDNumbering(0, 48);
    }

    public CharactorGenerator Ready()
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
