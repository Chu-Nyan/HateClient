using Chu.Utility;
using UnityEngine;

namespace Chu.Collision
{
    public class NyanColliderFactory : Singleton<NyanColliderFactory>
    {
        private NyanCollisonSystem _system;

        public void Init(NyanCollisonSystem sys)
        {
            if (_system != null)
            {
                Debug.LogError("Already initialized.");
                return;
            }

            _system = sys;
        }

        public NyanCollider Create(INyanCollisionProvider provider, IShape shape, bool isActive = true, string comment = default)
        {
            NyanCollider collider = new(shape, isActive, comment);
            _system.RegisterEntity(collider, provider);
            return collider;
        }
    }
}
