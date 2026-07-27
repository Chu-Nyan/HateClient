using System.Collections.Generic;
using UnityEngine;

namespace SAB.GameSystem
{
    public class MapReferenceContainer : MonoBehaviour
    {
        private Dictionary<int, Character> _characters;
        private Dictionary<int, string> _favoriteObjects;

        public Dictionary<int, Character> Characters
        {
            get => _characters;
        }
        public Dictionary<int, string> FavoriteObjects
        {
            get => _favoriteObjects;
        }

        public void AutoBinding()
        {
            _characters = new();
            _favoriteObjects = new();
            int id = 0;
            var acters = transform.GetComponentsInChildren<Character>();

            foreach (var item in acters)
            {
                id++;
                _characters.Add(id, item);
                if (item.TryGetComponent<ObjectMarker>(out var marker) == true)
                {
                    _favoriteObjects.Add(id, marker.FavoriteID);
                }
            }
        }
    }
}
