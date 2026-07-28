using Chu.Collision;
using Chu.Data;
using Chu.Tools;
using Chu.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Core
{
    public class ChuEngine : Singleton<ChuEngine>
    {
        private readonly GameObject _root;

        private readonly TextTable<string> _textResource;
        private NyanCollisonSystem _collisionSys;

        public TextTable<string> TextResource
        {
            get => _textResource;
        }

        public ChuEngine(string language) : base()
        {
            _root = new GameObject("ChuEngine");
            _textResource = new();

            var json = ExternalFolderHandler.GetLanguagesTextFile(language);
            _textResource.LoadTexts(JsonConvert.DeserializeObject<Dictionary<string, string>>(json));
        }

        public void ActivateCollisionSystem(RectBound bound, int capacity)
        {
            _collisionSys = _root.AddComponent<NyanCollisonSystem>();
            _collisionSys.InitArray(capacity);
            _collisionSys.InitBound(bound);

            var nyanColliderGenerator = new NyanColliderFactory();
            nyanColliderGenerator.Init(_collisionSys);
        }

        public static void Run(string language)
        {
            new ChuEngine(language);
            Instance.ActivateCollisionSystem(new(0, 100, 0, 100), 20);
        }
    }
}
