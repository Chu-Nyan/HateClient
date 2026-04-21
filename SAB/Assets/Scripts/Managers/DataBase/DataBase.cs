using Chu.Utility;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class DataBase : Singleton<DataBase>
    {
        public readonly SkillRepository SkillRepo;
        public readonly CharacterRepository CharacterRepo;
        public readonly ItemRepository ItemRepo;
        public readonly CutSceneRepository CutSceneRepo;

        public DataBase()
        {
            SkillRepo = new SkillRepository();
            CharacterRepo = new CharacterRepository();
            ItemRepo = new ItemRepository();
            CutSceneRepo = new CutSceneRepository();
        }

        public static  Dictionary<K, V[]> DeserializeArrayByKey<TDTO, K, V>(TDTO[] dtos, Func<TDTO, K> keySelector, Func<TDTO, V> converter)
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
    }
}
