using Chu.Data;
using Chu.Utility;
using SAB.Cutscene;
using SAB.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.GameSystem
{
    public class MapReferenceBinding : MonoBehaviour
    {
        public Transform CutsceneTriggerRoot;
        public Transform[] SpawnPoint;

        public MapGeneratorData GetGeneratorData()
        {
            var id = 0;
            var actors = new Dictionary<int, CharacterSpawnRequest>();
            var spawnPoint = new Pose2D[SpawnPoint.Length];
            var favorites = new Dictionary<int, string>();
            var uniqueTypes = new Dictionary<int, UniqueObjType>();

            foreach (var actor in transform.GetComponentsInChildren<Character>())
            {
                id++;
                var spwanData = new CharacterSpawnRequest(actor.Stats.CharacterID, actor.BrainType, actor.transform.position, actor.transform.rotation);
                actors.Add(id, spwanData);
                if (actor.TryGetComponent<ObjectMarker>(out var marker) == true)
                {
                    if (string.IsNullOrEmpty(marker.FavoriteID) == false)
                        favorites.Add(id, marker.FavoriteID);
                    if (marker.UniqueType != UniqueObjType.None)
                        uniqueTypes.Add(id, marker.UniqueType);
                }
            }

            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                spawnPoint[i] = SpawnPoint[i].ToPose2D();
            }

            return new MapGeneratorData
            {
                Characters = actors,
                SpawnPoint = spawnPoint,
                CutsceneTriggers = GetTriggerData(),
                Favorites = favorites,
                UniqueObjs = uniqueTypes,
            };
        }

        private CutsceneTriggerData[] GetTriggerData()
        {
            List<CutscenePreset> directors = CutsceneTriggerRoot.GetComponentsWithDepth<CutscenePreset>(1);
            var datas = new CutsceneTriggerData[directors.Count];

            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = directors[i].ToData();
            }

            return datas;
        }
    }
}
