using System.Collections.Generic;
using UnityEngine;

public class AniStateBehaviour
{
    private readonly List<AniClipEvent> _enterEvents;
    private readonly List<AniClipEvent> _progressEvents;
    private readonly List<AniClipEvent> _exitEvents;
    private int _playIndex;
    private float _beforeNormalizedTime;

    public AniStateBehaviour()
    {
        _enterEvents = new();
        _progressEvents = new();
        _exitEvents = new();
    }

    public void RegisterEvent(AniClipEvent clipEvent)
    {
        if (clipEvent.Timeing <= 0)
            _enterEvents.Add(clipEvent);
        else if (clipEvent.Timeing >= 1)
            _exitEvents.Add(clipEvent);
        else
        {
            _progressEvents.Add(clipEvent);
            _progressEvents.Sort((a, b) => a.Timeing.CompareTo(b.Timeing));
        }
    }

    public void OnStateEnter(AnimatorStateInfo stateInfo)
    {
        ResetEvent();
        for (int i = 0; i < _enterEvents.Count; i++)
        {
            _enterEvents[i].TryExecute(0);
        }
    }

    public void OnStateUpdate(AnimatorStateInfo stateInfo)
    {
        if (_playIndex >= _progressEvents.Count)
            return;
        if (_beforeNormalizedTime > stateInfo.normalizedTime)
            ResetEvent();

        bool isSucceed = true;
        while (isSucceed == true && _playIndex < _progressEvents.Count)
        {
            isSucceed = _progressEvents[_playIndex].TryExecute(stateInfo.normalizedTime);
            if (isSucceed == true)
                _playIndex++;
        }

        _beforeNormalizedTime = stateInfo.normalizedTime;
    }

    public void OnStateExit(AnimatorStateInfo stateInfo)
    {
        for (int i = 0; i < _exitEvents.Count; i++)
        {
            _exitEvents[i].TryExecute(1);
        }
    }

    private void ResetEvent()
    {
        _playIndex = 0;
    }
}
