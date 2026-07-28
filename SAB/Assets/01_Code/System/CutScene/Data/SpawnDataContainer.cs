using System;
using System.Collections;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class SpawnDataContainer
    {
        public Dictionary<Type, IDictionary> _datasByType;
        public Dictionary<Type, IDictionary> _externalObjectByType;

        public SpawnDataContainer()
        {
            _datasByType = new();
            _externalObjectByType = new();
        }

        public bool TryGetTable<T>(out Dictionary<int, T> dic) where T : ICutscenePreset
        {
            var type = typeof(T);
            dic = null;
            if (_datasByType.ContainsKey(type) == true)
            {
                dic = (Dictionary<int, T>)_datasByType[type];
            }
            if (_externalObjectByType.ContainsKey(type) == true)
            {
                dic = (Dictionary<int, T>)_externalObjectByType[type];
            }

            return dic != null;
        }

        public void Add<T>(int id, T data, bool isExternal) where T : ICutscenePreset
        {
            var type = typeof(T);
            var targetDic = isExternal == true ? _externalObjectByType : _datasByType;
            var table = TryGetValue<T>(targetDic, type);
            table[id] = data;
        }

        private IDictionary TryGetValue<T>(Dictionary<Type, IDictionary> dic, Type type) where T : ICutscenePreset
        {
            if (dic.TryGetValue(type, out var table) == false)
            {
                table = new Dictionary<int, T>();
                dic[type] = table;
            }

            return table;
        }

        public IEnumerator<KeyValuePair<int, ICutscenePreset>> GetEnumerator()
        {
            foreach (var table in _datasByType.Values)
            {
                foreach (DictionaryEntry entry in table)
                {
                    yield return new KeyValuePair<int, ICutscenePreset>((int)entry.Key, (ICutscenePreset)entry.Value);
                }
            }
        }
    }
}
