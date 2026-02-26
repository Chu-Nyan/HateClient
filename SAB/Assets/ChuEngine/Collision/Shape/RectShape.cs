using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    public class RectShape : Shape
    {
        private float _width;
        private float _height;
        private float _degree;
        private float _radian;
        private readonly Vector2[] _axis = new Vector2[2];      // 0 : Right, 1 : Up
        private readonly Vector2[] _radius = new Vector2[2];    // 0 : Right, 1 : Up

        public Vector2[] Axis
        {
            get => _axis;
        }

        public Vector2[] Radius
        {
            get => _radius;
        }

        public RectShape(float width, float height, float degree, Vector2 pos) : base(ShapeType.Rectangle)
        {
            _width = width;
            _height = height;
            _aabb = new(0, width, 0, height);
            UpdateAABB(pos, degree);
        }

        public override void UpdateAABB(Vector2 position, float degree)
        {
            _aabb.RefreshPosition(position);
            RefreshAxis(degree);
        }

        private void RefreshAxis(float degree)
        {
            if (_degree == degree)
                return;

            _degree = degree;
            _radian = degree * Mathf.Deg2Rad;

            float cos = Mathf.Cos(_radian);
            float sin = Mathf.Sin(_radian);
            _axis[0] = new Vector2(-sin, cos);
            _axis[1] = new Vector2(cos, sin);
            _radius[0] = _width * 0.5f * _axis[1];
            _radius[1] = _height * 0.5f * _axis[0];
        }

        public override bool Intersects(Shape target)
        {
            if (RectBound.IsIntersecting(AABB, target.AABB) == false)
                return false;

            return target.Intersects(this);
        }

        public override bool Intersects(RectShape shape)
        {
            return CollisionHelper.IsColliding(this, shape);
        }

        public override bool Intersects(CircleShape shape)
        {
            return CollisionHelper.IsColliding(this, shape);
        }

        public override bool Intersects(CompositeShape shape)
        {
            return CollisionHelper.IsColliding(shape, this);
        }
    }
}
