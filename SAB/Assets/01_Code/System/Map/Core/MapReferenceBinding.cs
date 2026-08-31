using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.GameSystem
{
    public class MapReferenceBinding : MonoBehaviour
    {
        [NonSerialized]
        public Dictionary<int, Character> Characters;
        [NonSerialized]
        public Dictionary<int, ObjectMarker> ObjMarker;
        public Transform[] SpawnPoint;

        public void AutoBinding()
        {
            Characters = new();
            ObjMarker = new();
            int id = 0;
            var acters = transform.GetComponentsInChildren<Character>();

            foreach (var item in acters)
            {
                id++;
                Characters.Add(id, item);
                if (item.TryGetComponent<ObjectMarker>(out var marker) == true)
                {
                    ObjMarker.Add(id, marker);
                }
            }
        }

        public Dictionary<int, string> GetFavoriteIDs()
        {
            var dic = new Dictionary<int, string>();
            foreach (var item in ObjMarker)
            {
                if (string.IsNullOrEmpty(item.Value.FavoriteID) == true)
                    continue;

                dic.Add(item.Key, item.Value.FavoriteID);
            }

            return dic;
        }

        public Dictionary<int, UniqueObjType> GetUniqueObjTypes()
        {
            var dic = new Dictionary<int, UniqueObjType>();
            foreach (var item in ObjMarker)
            {
                if (item.Value.UniqueType == UniqueObjType.None)
                    continue;

                dic.Add(item.Key, item.Value.UniqueType);
            }

            return dic;
        }
    }
}
