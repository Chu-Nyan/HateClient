using System;
using System.Collections;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class ObjectDataContainer
    {
        public Dictionary<Type, IDictionary> _datasByType;

        public ObjectDataContainer()
        {
            _datasByType = new();
        }

        public Dictionary<int, T> GetTable<T>() where T : IObjectConfig
        {
            var type = typeof(T);

            if (_datasByType.TryGetValue(type, out var table) == false)
            {
                table = new Dictionary<int, T>();
                _datasByType[type] = table;
            }

            return (Dictionary<int, T>)table;
        }

        public void Add<T>(int id, T data) where T : IObjectConfig
        {
            var table = GetTable<T>();
            table[id] = data;
        }

        public IEnumerator<KeyValuePair<int, IObjectConfig>> GetEnumerator()
        {
            foreach (var table in _datasByType.Values)
            {
                foreach (DictionaryEntry entry in table)
                {
                    yield return new KeyValuePair<int, IObjectConfig>(
                        (int)entry.Key,
                        (IObjectConfig)entry.Value);
                }
            }
        }
    }
}
