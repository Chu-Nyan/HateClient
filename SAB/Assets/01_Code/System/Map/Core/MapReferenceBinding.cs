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
        public Dictionary<int, string> FavoriteObjects;
        public Transform[] SpawnPoint;

        public void AutoBinding()
        {
            Characters = new();
            FavoriteObjects = new();
            int id = 0;
            var acters = transform.GetComponentsInChildren<Character>();

            foreach (var item in acters)
            {
                id++;
                Characters.Add(id, item);
                if (item.TryGetComponent<ObjectMarker>(out var marker) == true)
                {
                    FavoriteObjects.Add(id, marker.FavoriteID);
                }
            }
        }
    }
}
