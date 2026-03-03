using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    public class RectShape : IShape
    {
        private RectRangeData _rectData;
        private RectBound _aabb;

        private float _objDegree;
        private float _degree = float.MinValue;
        private float _radian = float.MinValue;

        private readonly Vector2[] _axis = new Vector2[2];      // 0 : Right, 1 : Up
        private readonly Vector2[] _radius = new Vector2[2];    // 0 : Right, 1 : Up

        public ShapeType ShapeType
        {
            get => ShapeType.Rectangle;
        }

        public Vector2[] Axis
        {
            get => _axis;
        }

        public Vector2[] Radius
        {
            get => _radius;
        }

        public RectBound AABB
        {
            get => _aabb;
        }

        public RectShape(RectRangeData data, Vector2 pos, float degree)
        {
            Setup(data, pos, degree);
        }

        public void Setup(RectRangeData data, Vector2 pos, float degree)
        {
            _rectData = data;
            _aabb = new(0, data.Width, 0, data.Height);
            UpdateAABB(pos, _rectData.Rotation + degree);
        }

        public void UpdateAABB(Vector2 position, float degree)
        {
            RefreshAxis(degree);
            _aabb.RefreshAABB(position, _radius);
        }

        private void RefreshAxis(float degree)
        {
            if (_objDegree == degree)
                return;

            _objDegree = degree;
            _degree = degree + _rectData.Rotation;
            _radian = _degree * Mathf.Deg2Rad;

            float cos = Mathf.Cos(_radian);
            float sin = Mathf.Sin(_radian);
            _axis[0] = new Vector2(-sin, cos);
            _axis[1] = new Vector2(cos, sin);
            _radius[0] = _rectData.Width * 0.5f * _axis[1];
            _radius[1] = _rectData.Height * 0.5f * _axis[0];
        }

        public bool Intersects(IShape target)
        {
            if (RectBound.IsIntersecting(AABB, target.AABB) == false)
                return false;

            return target.Intersects(this);
        }

        public bool Intersects(RectShape shape)
        {
            return CollisionHelper.IsColliding(this, shape);
        }

        public bool Intersects(CircleShape shape)
        {
            return CollisionHelper.IsColliding(this, shape);
        }

        public bool Intersects(CompositeShape shape)
        {
            return CollisionHelper.IsColliding(shape, this);
        }
    }
}
