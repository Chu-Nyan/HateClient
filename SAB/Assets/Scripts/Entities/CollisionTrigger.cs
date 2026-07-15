using Chu.Collision;
using Chu.Data;
using Chu.Utility;
using System;

namespace SAB.Cutscene
{
    public class CollisionTrigger : INyanCollisionProvider
    {
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
            ID = UniqueIDProvider.Next();
            _collider = GameColliderFactory.CreateCutsceneTrigger(this, ID);
        }

        public void Setup(IShape shape, Pose2D pose2D, bool isActive)
        {
            _collider.SetShape(shape);
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
