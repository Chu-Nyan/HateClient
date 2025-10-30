using Chu.Data;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Chu.Collision
{
    public class RectShape : Shape
    {
        private float _rotation;
        private float _axisUpdatedRotation = float.MaxValue;
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

        public override RectBound WorldRectBound
        {
            get => _bound.GetWorldCorners(_radius);
        }

        public RectShape(float rotation, float width, float height) : base(ShapeType.Rectangle, 0, width, 0, height)
        {
            _rotation = rotation;
        }

        public RectShape(float minX, float maxX, float minY, float maxY, float rotation = 0) : base(ShapeType.Rectangle, minX, maxX, minY, maxY)
        {
            _rotation = rotation;
        }

        public override void UpdateRectBound(Transform transform)
        {
            base.UpdateRectBound(transform);
            _rotation = transform.localRotation.eulerAngles.y * Mathf.Deg2Rad;
            RefreshAxis();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RefreshAxis()
        {
            if (_axisUpdatedRotation == _rotation)
                return;

            float cos = Mathf.Cos(_rotation);
            float sin = Mathf.Sin(_rotation);
            _axis[0] = new Vector2(-sin, cos);
            _axis[1] = new Vector2(cos, sin);
            _radius[0] = _axis[1] * _bound.HalfX;
            _radius[1] = _axis[0] * _bound.HalfY;
            _axisUpdatedRotation = _rotation;
        }

        public override bool Intersects(Shape target)
        {
            if (RectBound.IsIntersecting(WorldRectBound, target.WorldRectBound) == false)
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
    }

}
