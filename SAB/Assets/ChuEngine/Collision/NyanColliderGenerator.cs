using Chu.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        public NyanColliderGenerator GenerateCollider(INyanCollisionProvider provider, Transform transform)
        {
            if (_initialized == false)
                throw new Exception("초기화 되지 않음");

            _newCollider = new NyanCollider(transform, _iDNumbering.GetID());
            _system.RegisterEntity(provider);
            return this;
        }

        public NyanColliderGenerator SetRectCollider(float rotation, float width, float height, string comment)
        {
            if (_initialized == false)
                throw new Exception("초기화 되지 않음");

            var rect = (RectShape)_pools[ShapeType.Rectangle].Dequeue();
            rect.Refresh(rotation, width, height);
            _newCollider.SetShape(rect, comment);
            return this;
        }

        public NyanColliderGenerator SetCircleCollider(float radius, string comment)
        {
            var circle = (CircleShape)_pools[ShapeType.Circle].Dequeue();
            circle.Refresh(radius);
            _newCollider.SetShape(circle, comment);
            return this;
        }

        public NyanColliderGenerator SetShape(Shape shape, string comment)
        {
            _newCollider.SetShape(shape, comment);
            return this;
        }

        public NyanCollider GetCollider()
        {
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
            collider.SetShape(_pools[afterType].Dequeue(), comment);
        }
        #endregion
    }
}