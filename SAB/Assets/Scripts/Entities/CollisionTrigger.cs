using Chu.Collision;
using Chu.Core;
using Chu.Data;
using System;

namespace SAB.Cutscene
{
    public class CollisionTrigger : INyanCollisionProvider
    {
        private static int _idCount = 1;

        public readonly int ID;
        private readonly NyanCollider _collider;
        private bool _isActive = false;

        private Action<int> _entered;

        public NyanCollider Collider
        {
            get => _collider;
        }

        public CollisionTrigger()
        {
            ID = _idCount;
            _idCount++;

            _collider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
                 .GenerateCollider(this, $"ColliderTrigger {ID}")
                 .SetInstigatorID(GetHashCode())
                 .GetCollider(_isActive);
        }

        public void Setup(IShape shape, Pose2D pose2D, NyanLayer layer, NyanLayerMask mask, bool isActive)
        {
            _collider.SetShape(shape);
            _collider.SetLayer(layer, mask);
            _collider.SetTransform(pose2D);
            SetActive(isActive);
        }

        public void SetActive(bool value)
        {
            if (_isActive == value)
                return;

            _collider.SetActive(value);
            _isActive = value;
        }

        public void RegisterOnEntered(Action<int> action)
        {
            _entered += action;
        }

        public void OnNyanCollisionEnter(INyanCollisionProvider provider)
        {
            _entered.Invoke(ID);
        }

        public void OnNyanCollisionExit(INyanCollisionProvider provider)
        {

        }

    }
}
