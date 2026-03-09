using Chu.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Collision
{
    public class ShapeFactory : Singleton<ShapeFactory>
    {
        private readonly Dictionary<Type, ObjectPooling<IShape>> _pool;

        public ShapeFactory()
        {
            _pool = new()
            {
                { typeof(RectShape), new(() => new RectShape()) },
                { typeof(CircleShape), new(() => new CircleShape()) },
                { typeof(CompositeShape), new(() => new CompositeShape()) }
            };
        }

        public T Generate<T>() where T : IShape
        {
            return (T)_pool[typeof(T)].Dequeue();
        }

        public void Release<T>(T shape) where T : IShape
        {
            if (shape == null)
                return;

            _pool[shape.GetType()].Enqueue(shape);
        }

        public void Release(List<IShape> shapes)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                Release(shapes[i]);
            }
        }

        public void ConvertShapesNonAlloc(ReadOnlySpan<ShapeParam> list, List<IShape> shapes)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Type == ShapeType.Composite)
                    Debug.Log("잘못된 타입 입력됨");
                else if (list[i].Type == ShapeType.Rectangle)
                    shapes.Add(ConvertRectShape(list[i]));
                else if (list[i].Type == ShapeType.Circle)
                    shapes.Add(ConvertCircleShape(list[i]));
            }
        }

        public RectShape ConvertRectShape(ShapeParam param)
        {
            var rectShape = Instance.Generate<RectShape>();
            var data = param.GetRectData();
            rectShape.Setup(data);
            return rectShape;
        }

        public CircleShape ConvertCircleShape(ShapeParam param)
        {
            var circleShape = Instance.Generate<CircleShape>();
            var data = param.GetCircleData();
            circleShape.Setup(data);
            return circleShape;
        }
    }
}
