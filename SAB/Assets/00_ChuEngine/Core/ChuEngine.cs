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
        private readonly NyanCollisonSystem _collisionSys;

        public TextTable<string> TextResource
        {
            get => _textResource;
        }

        public ChuEngine(EngineSetting setting) : base()
        {
            _root = new GameObject("ChuEngine");
            _textResource = new();
            var json = ExternalFolderHandler.GetLanguagesTextFile(setting.Language);
            _textResource.LoadTexts(JsonConvert.DeserializeObject<Dictionary<string, string>>(json));

            if (setting.UseCollision == true)
            {
                _collisionSys = RunCollisionSystem(setting.CollisionBound, setting.Capacity);
            }
        }

        public static void Run(EngineSetting setting)
        {
            new ChuEngine(setting);
        }

        private NyanCollisonSystem RunCollisionSystem(RectBound bound, int capacity)
        {
            var collisionSys = _root.AddComponent<NyanCollisonSystem>();
            collisionSys.InitArray(capacity);
            collisionSys.InitBound(bound);

            var nyanColliderGenerator = new NyanColliderFactory();
            nyanColliderGenerator.Init(collisionSys);
            return collisionSys;
        }
    }
}
