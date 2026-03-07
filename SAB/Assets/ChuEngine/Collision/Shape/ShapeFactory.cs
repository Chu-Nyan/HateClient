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

            _pool[typeof(T)].Enqueue(shape);
        }
    }
}
