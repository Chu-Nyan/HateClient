using Chu.Utility;
using System;
using System.Collections.Generic;

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

        public static IShape ConverShape(ShapeParam param)
        {
            if (param.Type == ShapeType.Rectangle)
                return ConvertRectShape(param);
            if (param.Type == ShapeType.Circle)
                return ConvertCircleShape(param);

            throw new Exception("잘못된 타입 입력됨");
        }

        public static IShape GenerateShape(ShapeParam[] param)
        {
            if (param.Length == 1)
                return ConverShape(param[0]);
            else
            {
                var composite = Instance.Generate<CompositeShape>();
                composite.Setup(param);
                return composite;
            }
        }

        public static void ConvertShapesNonAlloc(ReadOnlySpan<ShapeParam> list, List<IShape> shapes)
        {
            for (int i = 0; i < list.Length; i++)
            {
                shapes.Add(ConverShape(list[i]));
            }
        }

        private static RectShape ConvertRectShape(ShapeParam param)
        {
            var rectShape = Instance.Generate<RectShape>();
            var data = param.GetRectData();
            rectShape.Setup(data);
            return rectShape;
        }

        private static CircleShape ConvertCircleShape(ShapeParam param)
        {
            var circleShape = Instance.Generate<CircleShape>();
            var data = param.GetCircleData();
            circleShape.Setup(data);
            return circleShape;
        }
    }
}
