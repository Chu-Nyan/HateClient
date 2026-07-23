using Chu.Core;
using Chu.Utility;
using System;
using System.Collections.Generic;

namespace SAB.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private readonly Dictionary<Type, BaseUI> _baseUIByID;

        public UIManager() : base()
        {
            _baseUIByID = new();
        }

        public T GetUI<T>() where T : BaseUI, new()
        {
            if (_baseUIByID.TryGetValue(typeof(T), out var ui) == false)
            {
                ui = AssetManager.GenerateLoadAssetSync<T>(typeof(T).ToString());
                _baseUIByID[typeof(T)] = ui;
            }

            return (T)ui;
        }
    }
}
