using Chu.Core;
using Chu.Utility;
using System;
using System.Collections.Generic;

namespace SAB.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private static readonly Dictionary<Type, string> _assetPathByType = new()
        {
            { typeof(SpeechBubbleUI),"SpeechBubbleUI" }
        };

        private readonly Dictionary<Type, UIController> _uiByID;

        public UIManager() : base()
        {
            _uiByID = new();
        }

        public T GetUI<T>() where T : UIController
        {
            Type type = typeof(T);
            if (_uiByID.TryGetValue(type, out var ui) == false)
            {
                if (_assetPathByType.TryGetValue(type, out string path) == false)
                    throw new InvalidOperationException($"path not registered\n{type.Name}");

                ui = AssetManager.InstantiateAssetSync<T>(path);
                _uiByID[type] = ui;
            }

            return (T)ui;
        }
    }
}
