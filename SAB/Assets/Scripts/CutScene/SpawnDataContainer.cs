using System;
using System.Collections;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class SpawnDataContainer
    {
        public Dictionary<Type, IDictionary> _datasByType;

        public SpawnDataContainer()
        {
            _datasByType = new();
        }

        public Dictionary<int, T> GetTable<T>() where T : ICutscenePreset
        {
            var type = typeof(T);

            if (_datasByType.TryGetValue(type, out var table) == false)
            {
                table = new Dictionary<int, T>();
                _datasByType[type] = table;
            }

            return (Dictionary<int, T>)table;
        }

        public void Add<T>(int id, T data) where T : ICutscenePreset
        {
            var table = GetTable<T>();
            table[id] = data;
        }

        public IEnumerator<KeyValuePair<int, ICutscenePreset>> GetEnumerator()
        {
            foreach (var table in _datasByType.Values)
            {
                foreach (DictionaryEntry entry in table)
                {
                    yield return new KeyValuePair<int, ICutscenePreset>(
                        (int)entry.Key,
                        (ICutscenePreset)entry.Value);
                }
            }
        }
    }
}
