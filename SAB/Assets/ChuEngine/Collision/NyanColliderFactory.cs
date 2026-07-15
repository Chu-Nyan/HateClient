using Chu.Utility;
using System;
using UnityEngine;

namespace Chu.Collision
{
    /// <summary>
    /// 시스템에 등록된 NyanCollider 생성
    /// </summary>
    public class NyanColliderFactory
    {
        private static NyanColliderFactory _instance;
        private readonly IDNumbering _iDNumbering;
        private NyanCollisonSystem _system;

        internal NyanColliderFactory()
        {
            if (_instance != null)
                return;

            _instance = this;
            _iDNumbering = new(0, 64);
        }

        internal void Init(NyanCollisonSystem sys)
        {
            if (_system != null)
            {
                Debug.LogError("Already initialized.");
                return;
            }

            _system = sys;
        }

        public static NyanCollider Create(INyanCollisionProvider provider, IShape shape, bool isActive = true, string comment = default)
        {
            if (_instance == null)
                throw new NullReferenceException();

            NyanCollider collider = new(_instance._iDNumbering.GetID(), provider, shape, isActive, comment);
            _instance._system.RegisterEntity(collider);
#if UNITY_EDITOR
            VerifyCollider(collider);
#endif
            return collider;
        }

#if UNITY_EDITOR
        private static void VerifyCollider(NyanCollider collider)
        {

            if (collider.IsValid(out var log) == false)
            {
                Debug.LogError(log);
            }
        }
#endif
    }
}
