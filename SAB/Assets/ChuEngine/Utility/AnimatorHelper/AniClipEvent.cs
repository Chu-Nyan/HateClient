using System;

public class AniClipEvent
{
    private bool _isExecuted;
    private float _timeing;
    private AniEventData _data;
    private Action<AniEventData> _action;

    public void Setup(AniEventData data, float timing, Action<AniEventData> action)
    {
        _timeing = timing;
        _data = data;
        _action = action;
    }

    public void Refresh()
    {
        _isExecuted = false;
    }

    public bool Tick(float normalizedTime)
    {
        if (_isExecuted == true && _timeing > normalizedTime)
            _isExecuted = false;

        if (_isExecuted == true)
            return false;

        if (_timeing > normalizedTime)
            return false;

        _isExecuted = true;
        _action?.Invoke(_data);
        return true;
    }
}
