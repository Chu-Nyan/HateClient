public interface IUIPresenter
{
    public string ViewAssetPath { get; }

    public void Init(UIView view);
    public void Show();
    public void Hide();
}
