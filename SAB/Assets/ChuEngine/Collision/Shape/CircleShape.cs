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

        public override RectBound WorldRectBound
        {
            get => _bound;
        }

        public CircleShape(float radius) : base(ShapeType.Circle, -radius, radius, -radius, radius)
        {
            _radius = radius;
        }

        public override bool Intersects(Shape target)
        {
            if (RectBound.IsIntersecting(_bound, target.WorldRectBound) == false)
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
    }
}
