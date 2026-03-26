using System.Collections.Generic;
using UnityEngine;

public partial class SpeechBubbleUIModel
{
    private readonly Dictionary<int, Transform> _anchorByID;
    private readonly Dictionary<int, DialogueData[]> _textByID;
    private readonly Dictionary<int, DialogueProgress> _progressByID;

    private readonly HashSet<int> _finishedIDThisFrame;
    private readonly HashSet<int> _stepChangedIDThisFrame;

    public Dictionary<int, Transform> AnchorByID
    {
        get => _anchorByID;
    }

    public HashSet<int> FinishedIDThisFrame
    {
        get => _finishedIDThisFrame;
    }

    public HashSet<int> StepChangedIDThisFrame
    {
        get => _stepChangedIDThisFrame;
    }

    public SpeechBubbleUIModel()
    {
        _anchorByID = new();
        _progressByID = new();
        _textByID = new();
        _finishedIDThisFrame = new();
        _stepChangedIDThisFrame = new();
    }

    public void Tick(float time)
    {
        foreach (var item in _progressByID)
        {
            var p = item.Value;
            p.RemainTime -= time;

            if (p.RemainTime <= 0)
            {
                p.ProgressStep++;
                if (p.ProgressStep < _textByID[p.ID].Length)
                {
                    p.RemainTime = _textByID[p.ID][p.ProgressStep].Time;
                    _stepChangedIDThisFrame.Add(p.ID);
                }
                else
                {
                    _finishedIDThisFrame.Add(p.ID);
                }
            }
        }
    }

    public bool AddDialogue(int id, Transform anchor, DialogueData[] text)
    {
        var result = _anchorByID.ContainsKey(id) == false;
        if (result == true)
        {
            _anchorByID[id] = anchor;
            _anchorByID[id] = anchor;
            _textByID[id] = text; // TODO 재활용
            _progressByID[id] = new DialogueProgress(id, 0, text[0].Time);
        }

        return result;
    }

    public void PostTick()
    {
        foreach (var id in _finishedIDThisFrame)
        {
            _anchorByID.Remove(id);
            _textByID.Remove(id);
            _progressByID.Remove(id);
        }

        _finishedIDThisFrame.Clear();
        _stepChangedIDThisFrame.Clear();
    }

    public DialogueData GetCurrentStepDialogue(int id)
    {
        int step = _progressByID[id].ProgressStep;
        if (step >= _textByID[id].Length)
        {
            step = _textByID[id].Length - 1;
            Debug.Log("스텝 계산 오류");
        }

        return _textByID[id][step];
    }
}
