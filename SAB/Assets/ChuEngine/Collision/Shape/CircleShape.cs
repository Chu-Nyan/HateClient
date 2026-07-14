using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    public class CircleShape : IShape
    {
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

        public CircleShape() : this(CircleRangeData.Default) { }

        public CircleShape(CircleRangeData data)
        {
            Setup(data);
        }

        public void Setup(CircleRangeData data)
        {
            _data = data;
            _aabb = new(-_data.Radius, _data.Radius, -_data.Radius, _data.Radius);
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
