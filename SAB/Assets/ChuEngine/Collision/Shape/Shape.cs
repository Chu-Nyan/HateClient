using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    public abstract class Shape
    {
        public static readonly Shape Invalid = new CircleShape(0, Vector2.zero);

        public readonly ShapeType ShapeType;
        protected RectBound _aabb;

        public RectBound AABB
        {
            get => _aabb;
        }

        public Shape(ShapeType type)
        {
            ShapeType = type;
        }

        public abstract void UpdateAABB(Vector2 position, float degree);
        public virtual bool Intersects(Shape target)
        {
            return RectBound.IsIntersecting(_aabb, target.AABB) == false;
        }
        public abstract bool Intersects(RectShape shape);
        public abstract bool Intersects(CircleShape shape);
        public abstract bool Intersects(CompositeShape shape);
    }
}
