using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    public class CircleShape : Shape
    {
        private float _radius;

        public float Radius
        {
            get => _radius;
        }

        public CircleShape(float radius, Vector2 position) : base(ShapeType.Circle)
        {
            _radius = radius;
            Refresh(_radius, position);
        }

        public void Refresh(float radius, Vector2 position)
        {
            _radius = radius;
            _aabb = new(-_radius, _radius, -_radius, _radius);
            _aabb.RefreshPosition(position);
        }

        public override void UpdateAABB(Vector2 position, float degree)
        {
            _aabb.RefreshPosition(position);
        }

        public override bool Intersects(Shape target)
        {
            if (RectBound.IsIntersecting(AABB, target.AABB) == false)
                return false;

            return target.Intersects(this);
        }

        public override bool Intersects(RectShape shape)
        {
            return CollisionHelper.IsColliding(shape, this);
        }

        public override bool Intersects(CircleShape shape)
        {
            return CollisionHelper.IsColliding(shape, this);
        }

        public override bool Intersects(CompositeShape shape)
        {
            return CollisionHelper.IsColliding(shape, this);
        }
    }
}
