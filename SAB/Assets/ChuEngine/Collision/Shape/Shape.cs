using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    public abstract class Shape
    {
        public readonly ShapeType ShapeType;
        protected RectBound _bound;

        public RectBound RectBound
        {
            get => _bound;
        }

        public abstract RectBound WorldRectBound { get; }

        public Shape(ShapeType type, float minX, float maxX, float minY, float maxY)
        {
            ShapeType = type;
            UpdateRectBound(minX, maxX, minY, maxY);
        }

        public virtual void UpdatePosition(Transform transform)
        {
            _bound.RefreshPosition(transform.position);
        }

        protected void UpdateRectBound(float minX, float maxX, float minY, float maxY)
        {
            _bound = new(minX, maxX, minY, maxY);
        }

        public abstract bool Intersects(Shape target);
        public abstract bool Intersects(RectShape shape);
        public abstract bool Intersects(CircleShape shape);
    }
}

