public class SoundOptionData
{
    public const float DefaultBGMVolume = 0.75f;
    public const float DefaultSEVolume = 0.25f;
    public const float DefaultPitch = 0.05f;
    public const float DefaultFadeSpeed = 0.5f;

    private float _globalVolume = 1.0f;
    private float _bgmVolume = 1.0f;
    private float _seVolume = 1.0f;

    private float _finalyBGMVolume = 0.75f;
    private float _finalySEVolume = 0.25f;

    private bool _isOn;

    public float GlobalVolume
    {
        get => _finalyBGMVolume;
    }

    public float BgmVolume
    {
        get => _finalySEVolume;
    }

    public float SeVolume
    {
        get => _finalySEVolume;
    }

    public float FinalyBGMVolume
    {
        get => _finalyBGMVolume;
    }

    public float FinalySEVolume
    {
        get => _finalySEVolume;
    }

    public bool IsOn
    {
        get => _isOn;
    }

    public void SetSwitch(bool isOn)
    {
        _isOn = isOn;
    }

    public void SetGlobalVolume(float volume)
    {
        _globalVolume = volume;
        SetBGMVolume(volume);
        SetSEVolume(volume);
    }

    public void SetBGMVolume(float volume)
    {
        _bgmVolume = volume;
        _finalyBGMVolume = _bgmVolume * _globalVolume * DefaultBGMVolume;
    }

    public void SetSEVolume(float volume)
    {
        _seVolume = volume;
        _finalySEVolume = _seVolume * _globalVolume * DefaultSEVolume;
    }
}
