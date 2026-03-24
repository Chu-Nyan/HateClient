public class DialogueUI : IUIPresenter
{
    private readonly DialogueUIModel _model;
    private DialogueUIView _view;

    public string ViewAssetPath
    {
        get => "DialogueUIView";
    }

    public DialogueUI()
    {
        _model = new();
    }

    public void Init(UIView view)
    {
        if (view is not DialogueUIView dialogueView)
            throw new System.Exception("잘못된 UIView");

        _view = dialogueView;
    }

    public void Show()
    {
        _view.SetActive(true);
    }

    public void Hide()
    {
        _view.SetActive(false);
    }
}
