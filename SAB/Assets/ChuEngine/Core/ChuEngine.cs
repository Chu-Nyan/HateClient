using Chu.Collision;
using Chu.Data;
using Library.DesignPattern;
using UnityEngine;

namespace Chu
{
    public class ChuEngine : Singleton<ChuEngine>
    {
        private readonly GameObject _root;

        private CollisionSystem _collisionSys;
        private GeneratorHub _hub;

        public GeneratorHub GeneratorHub
        {
            get => _hub;
        }

        public ChuEngine(GameObject root) : base()
        {
            _root = root;
            InitGeneratorHub();
        }

        private void InitGeneratorHub()
        {
            _hub = new GeneratorHub();
        }

        public void ActivateCollisionSystem(RectBound bound, int capacity)
        {
            _collisionSys = _root.AddComponent<CollisionSystem>();
            _collisionSys.InitArray(capacity);
            _collisionSys.InitBound(bound);
        }
    }
}
