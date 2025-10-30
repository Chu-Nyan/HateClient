using Chu.Collision;
using Chu.Data;
using Chu.Utility;
using UnityEngine;

namespace Chu
{
    public class ChuEngine : Singleton<ChuEngine>
    {
        private readonly GameObject _root;

        private NyanCollisonSystem _collisionSys;
        private GeneratorHub _hub;

        public GeneratorHub GeneratorHub
        {
            get => _hub;
        }

        public ChuEngine(GameObject root) : base()
        {
            _root = root;
            _hub = new GeneratorHub();
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
