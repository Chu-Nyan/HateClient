using Chu.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Collision
{
    public class CompositeShape : Shape
    {
        private readonly List<Shape> _shapeList;

        public List<Shape> ShapeList
        {
            get => _shapeList;
        }

        public Shape this[int index]
        {
            get => _shapeList[index];
            set => _shapeList[index] = value;
        }

        public CompositeShape(List<Shape> list, Vector2 position, float degree) : base(ShapeType.Composite)
        {
            _shapeList = new();
            for (int i = 0; i < list.Count; i++)
            {
                _shapeList.Add(list[i]);
            }
            UpdateAABB(position, degree);
        }

        public override void UpdateAABB(Vector2 position, float degree)
        {
            if (_shapeList.Count == 0)
                return;

            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            for (int i = 0; i < _shapeList.Count; i++)
            {
                _shapeList[i].UpdateAABB(position, degree);
                var elementBound = _shapeList[i].AABB;

                if (elementBound.MinX < minX) minX = elementBound.MinX;
                if (elementBound.MaxX > maxX) maxX = elementBound.MaxX;
                if (elementBound.MinY < minY) minY = elementBound.MinY;
                if (elementBound.MaxY > maxY) maxY = elementBound.MaxY;
            }

            _aabb = new RectBound(minX, maxX, minY, maxY);
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
            return CollisionHelper.IsColliding(this, shape);
        }
    }
}
