using Chu.Utility;
using Chu.Utility.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class DataBase : Singleton<DataBase>
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings().WithUnity();

        public readonly SkillRepository SkillRepo;
        public readonly CharacterRepository CharacterRepo;
        public readonly ItemRepository ItemRepo;
        public readonly CutsceneRepository CutSceneRepo;

        public DataBase()
        {
            SkillRepo = new SkillRepository();
            CharacterRepo = new CharacterRepository();
            ItemRepo = new ItemRepository();
            CutSceneRepo = new CutsceneRepository();
        }

        public static Dictionary<K, V[]> DeserializeArrayByKey<TDTO, K, V>(TDTO[] dtos, Func<TDTO, K> keySelector, Func<TDTO, V> converter)
        {
            var dic = new Dictionary<K, V[]>();

            int i = 0;
            while (i < dtos.Length)
            {
                int start = i;
                K currentKey = keySelector(dtos[start]);

                while (i < dtos.Length && keySelector(dtos[i]).Equals(currentKey))
                {
                    i++;
                }

                int size = i - start;
                var array = new V[size];

                for (int j = 0; j < size; j++)
                {
                    array[j] = converter(dtos[start + j]);
                }

                dic[currentKey] = array;
            }

            return dic;
        }


        public static Dictionary<K, V> DeserializeObjectByKey<TDTO, K, V>(TDTO[] dtos, Func<TDTO, K> keySelector, Func<TDTO, V> converter)
        {
            var dic = new Dictionary<K, V>();

            for (int i = 0; i < dtos.Length; i++)
            {
                var data = converter(dtos[i]);
                dic.Add(keySelector(dtos[i]), data);
            }

            return dic;
        }

        public static T[] ConvertJsonToArray<T>(string json)
        {
            return JsonConvert.DeserializeObject<T[]>(json, JsonSerializerSettings);
        }
    }
}
