using Chu.Collision.Layer;
using Chu.Utility;
using System;
using System.Collections.Generic;

namespace Chu.Collision
{
    /// <summary>
    /// NyanCollider를 생성하고 충돌 시스템에 등록하는 팩토리
    /// </summary>
    public class NyanColliderGenerator
    {
        private readonly IDNumbering _iDNumbering;
        private readonly Dictionary<ShapeType, ObjectPooling<Shape>> _pools;
        private NyanCollisonSystem _system;
        private NyanCollider _newCollider;
        private bool _initialized;

        public NyanColliderGenerator()
        {
            _iDNumbering = new(0, 64);

            // 풀 초기화
            var rectPool = new ObjectPooling<Shape>(() => new RectShape(0, 1, 1));
            var circlePool = new ObjectPooling<Shape>(() => new CircleShape(1));
            _pools = new()
            {
                { ShapeType.Rectangle, rectPool },
                { ShapeType.Circle, circlePool }
            };
        }

        public void Init(NyanCollisonSystem sys)
        {
            if (_initialized == true)
                throw new Exception("초기화 중복 호출");

            _system = sys;
            _initialized = true;
        }

        #region 생성 함수
        public NyanColliderGenerator GenerateCollider(INyanCollisionProvider provider, Shape shape, string comment = default)
        {
            if (_initialized == false)
                throw new Exception("초기화 되지 않음");

            _newCollider = new NyanCollider(provider, shape, _iDNumbering.GetID(), comment);
            return this;
        }

        public NyanColliderGenerator SetLayer(NyanLayer layer, NyanLayerMask mask)
        {
            _newCollider.InitLayer(layer, mask);
            return this;
        }

        public NyanColliderGenerator SetInstigatorID(int id)
        {
            _newCollider.SetInstigatorID(id);
            return this;
        }

        public NyanCollider GetCollider()
        {
#if UNITY_EDITOR
            VerifyCollider(_newCollider);
#endif
            _system.RegisterEntity(_newCollider);
            return _newCollider;
        }
        #endregion

        #region 유틸리티
        public void ChangeShape(ShapeType afterType, NyanCollider collider, string comment = null)
        {
            var beforeType = collider.Shape.ShapeType;
            if (beforeType == afterType)
                return;

            comment ??= collider.Comment;
            _pools[beforeType].Enqueue(collider.Shape);
            collider.SetShape(_pools[afterType].Dequeue());
        }
        #endregion

#if UNITY_EDITOR
        private void VerifyCollider(NyanCollider collider)
        {
            if (collider.IsValid(out var log) == false)
            {
                throw new Exception(log);
            }
        }
#endif
    }
}