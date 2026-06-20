using UnityEngine;

public class SpeechBubbleUI : BaseUI
{
    [SerializeField]
    private SpeechBubbleUIView _view;
    private SpeechBubbleUIModel _model;

    private void Awake()
    {
        _model = new();
    }

    private void Update()
    {
        _model.Tick(Time.deltaTime);
        foreach (var id in _model.StepChangedIDThisFrame)
        {
            var dialogue = _model.GetCurrentStepDialogue(id);
            _view.ShowBubble(id, dialogue.Text);
        }
        foreach (var id in _model.FinishedIDThisFrame)
        {
            _view.HideBubble(id);
        }
        _model.PostTick();

        foreach (var item in _model.AnchorByID)
        {
            var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, item.Value.position);
            _view.SetBubblePosition(item.Key, pos);
        }
    }

    public void ShowDialogue(Transform anchor, DialogueData[] text)
    {
        var id = anchor.GetInstanceID();
        if (_model.AddDialogue(id, anchor, text) == true)
        {
            _view.ShowBubble(id, text[0].Text);
        }
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
