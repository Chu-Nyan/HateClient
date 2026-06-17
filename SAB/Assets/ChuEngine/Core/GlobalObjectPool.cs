using Chu.Utility;
using System;
using System.Collections.Generic;

namespace Chu.Core
{
    /// <summary>
    /// 전역으로 사용하는 오브젝트 풀
    /// </summary>
    public class GlobalObjectPool
    {
        private static GlobalObjectPool _instance;

        private Dictionary<Type, object> _pool;

        public GlobalObjectPool()
        {
            _instance = this;
            _pool = new();
        }

        public static void RegisterPool<T>(Func<T> generateFunction) where T : class
        {
            Dictionary<Type, object> globalPool = _instance._pool;
            Type type = typeof(T);

            if (globalPool.ContainsKey(type) == false)
            {
                var pool = new ObjectPooling<T>(generateFunction);
                globalPool.Add(type, pool);
            }
        }

        public static T Get<T>() where T : class
        {
            if (_instance._pool.TryGetValue(typeof(T), out object pool) == false)
                throw new InvalidOperationException($"{typeof(T)} : 풀에 등록 되지 않음");

            return ((ObjectPooling<T>)pool).Dequeue();
        }

        public static void Release<T>(T obj) where T : class
        {
            if (_instance._pool.TryGetValue(typeof(T), out object pool) == false)
                throw new InvalidOperationException($"{typeof(T)} : 풀에 등록 되지 않음");

            ((ObjectPooling<T>)pool).Enqueue(obj);
        }
    }
}
