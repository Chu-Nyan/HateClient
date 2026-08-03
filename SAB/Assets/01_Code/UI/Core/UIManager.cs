using Chu.Core;
using Chu.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private static readonly Dictionary<Type, string> _assetPathByType = new()
        {
            {typeof(SpeechBubbleUI),"SpeechBubbleUI" }
        };

        private readonly Dictionary<Type, UIBase> _baseUIByID;

        public UIManager() : base()
        {
            _baseUIByID = new();
        }

        public T GetUI<T>() where T : UIBase, new()
        {
            Type type = typeof(T);
            if (_baseUIByID.TryGetValue(type, out var ui) == false)
            {
                if (_assetPathByType.TryGetValue(type, out string path) == false)
                {
                    path = type.Name;
                    Debug.LogWarning($"{path}, not registered");
                }

                ui = AssetManager.GenerateLoadAssetSync<T>(path);
                _baseUIByID[typeof(T)] = ui;
            }

            return (T)ui;
        }
    }
}
