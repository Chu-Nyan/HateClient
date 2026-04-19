using Chu.Collision.Layer;
using Chu.Utility;
using System;
using UnityEngine;

namespace Chu.Collision
{
    /// <summary>
    /// NyanCollider를 생성하고 충돌 시스템에 등록하는 팩토리
    /// </summary>
    public class NyanColliderGenerator
    {
        private readonly IDNumbering _iDNumbering;
        private NyanCollisonSystem _system;
        private NyanCollider _newCollider;
        private bool _initialized;

        public NyanColliderGenerator()
        {
            _iDNumbering = new(0, 64);
        }

        public void Init(NyanCollisonSystem sys)
        {
            if (_initialized == true)
                throw new Exception("초기화 중복 호출");

            _system = sys;
            _initialized = true;
        }

        #region 생성 함수
        public NyanColliderGenerator GenerateCollider(INyanCollisionProvider provider, string comment = default)
        {
            if (_initialized == false)
                throw new Exception("초기화 되지 않음");

            _newCollider = new NyanCollider(provider, _iDNumbering.GetID(), comment);
            return this;
        }

        public NyanColliderGenerator SetShape(IShape shape)
        {
            _newCollider.SetShape(shape);

            return this;
        }

        public NyanColliderGenerator SetLayer(NyanLayer layer, NyanLayerMask mask)
        {
            _newCollider.SetLayer(layer, mask);
            return this;
        }

        public NyanColliderGenerator SetInstigatorID(int id)
        {
            _newCollider.SetInstigatorID(id);
            return this;
        }

        public NyanColliderGenerator SetTransform(Vector2 pos, float eulerY)
        {
            _newCollider.SetTransform(pos, eulerY);
            return this;
        }

        public NyanCollider GetCollider(bool isActivate)
        {
#if UNITY_EDITOR
            VerifyCollider(_newCollider);
#endif
            _system.RegisterEntity(_newCollider);
            _newCollider.SetActive(isActivate);
            return _newCollider;
        }
        #endregion

#if UNITY_EDITOR
        private void VerifyCollider(NyanCollider collider)
        {
            if (collider.IsValid(out var log) == false)
            {
                Debug.LogError(log);
            }
        }
#endif
    }
}
