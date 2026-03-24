using Chu.Utility;
using System;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<Type, IUIPresenter> _baseUIByID;

    public UIManager() : base()
    {
        _baseUIByID = new();
    }

    public T GetUI<T>() where T : IUIPresenter, new()
    {
        if (_baseUIByID.TryGetValue(typeof(T), out var ui) == false)
        {
            ui = new T();
            var view = AssetManager.GenerateLoadAssetSync<DialogueUIView>(ui.ViewAssetPath);
            ui.Init(view);
            _baseUIByID[typeof(T)] = ui;
        }

        ui.Show();
        return (T)ui;
    }
}
