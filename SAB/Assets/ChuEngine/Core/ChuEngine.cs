using Chu.Collision;
using Chu.Data;
using Chu.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Core
{
    public class ChuEngine : Singleton<ChuEngine>
    {
        private readonly GameObject _root;

        private NyanCollisonSystem _collisionSys;
        private TextResource<string> _textResource;
        private GlobalObjectPool _objectPool;
        private GeneratorHub _hub;

        public GeneratorHub GeneratorHub
        {
            get => _hub;
        }

        public ChuEngine(GameObject root, string language) : base()
        {
            _root = root;
            _textResource = new();
            _hub = new GeneratorHub();
            _objectPool = new GlobalObjectPool();

            var json = AssetManager.ExternalFolder.GetLanguagesTextFile(language);
            _textResource.LoadTexts(JsonConvert.DeserializeObject<Dictionary<string, string>>(json));
        }

        public void ActivateCollisionSystem(RectBound bound, int capacity)
        {
            _collisionSys = _root.AddComponent<NyanCollisonSystem>();

            _hub.InitNyanColliderGenerator(_collisionSys);
            _collisionSys.InitArray(capacity);
            _collisionSys.InitBound(bound);
        }
    }
}
