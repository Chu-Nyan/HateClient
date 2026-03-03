using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    public class CircleShape : IShape
    {
        public static readonly IShape Invalid = new CircleShape(new CircleRangeData(Vector3.zero, 1));

        private CircleRangeData _data;
        private RectBound _aabb;

        public float Radius
        {
            get => _data.Radius;
        }

        public ShapeType ShapeType
        {
            get => ShapeType.Circle;
        }

        public RectBound AABB
        {
            get => _aabb;
        }

        public CircleShape(CircleRangeData data)
        {
            Refresh(data, Vector3.zero);
        }

        public void Refresh(CircleRangeData data, Vector2 position)
        {
            _data = data;
            _aabb = new(-_data.Radius, _data.Radius, -_data.Radius, _data.Radius);
            _aabb.RefreshAABB(position);
        }

        public void UpdateAABB(Vector2 position, float degree)
        {
            _aabb.RefreshAABB(position);
        }

        public bool Intersects(IShape target)
        {
            if (RectBound.IsIntersecting(AABB, target.AABB) == false)
                return false;

            return target.Intersects(this);
        }

        public bool Intersects(RectShape shape)
        {
            return CollisionHelper.IsColliding(shape, this);
        }

        public bool Intersects(CircleShape shape)
        {
            return CollisionHelper.IsColliding(shape, this);
        }

        public bool Intersects(CompositeShape shape)
        {
            return CollisionHelper.IsColliding(shape, this);
        }
    }
}
