using Chu.Collision;
using Chu.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace Chu
{
    public class ChuEngine
    {
        private Transform _root;

        private CollisionSystem _collisionSys;

        public CollisionSystem CollisionSystem 
        { 
            get => _collisionSys;
        }

        public ChuEngine(Transform root)
        {
            _root = root;
        }

        public void ActivateCollisionSystem(RectBound bound, int capacity)
        {
            _collisionSys = _root.AddComponent<CollisionSystem>();
            _collisionSys.InitArray(capacity);
            _collisionSys.InitBound(bound);
        }
    }
}
