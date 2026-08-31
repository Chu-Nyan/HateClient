using Chu.Data;
using Chu.Utility;
using SAB.Cutscene;
using SAB.Unit;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SAB.GameSystem
{
    public class MapReferenceBinding : DataExporter
    {
        [SerializeField]
        private MapType _mapType;
        [SerializeField]
        private DefaultAsset _generatorFolder;
        [SerializeField]
        private Transform _cutsceneTriggerRoot;
        [SerializeField]
        private Transform[] _spawnPoint;

        protected override string DataPath
        {
            get => Path.Combine(AssetDatabase.GetAssetPath(_generatorFolder), string.Format(Const.Asset_DB_MapGenerated, _mapType));
        }

        private MapGeneratorData GetGeneratorData()
        {
            var id = 0;
            var actors = new Dictionary<int, CharacterSpawnRequest>();
            var spawnPoint = new Pose2D[_spawnPoint.Length];
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

            for (int i = 0; i < _spawnPoint.Length; i++)
            {
                spawnPoint[i] = _spawnPoint[i].ToPose2D();
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

        protected override object GenerateData()
        {
            return GetGeneratorData();
        }

        private CutsceneTriggerData[] GetTriggerData()
        {
            List<CutscenePreset> directors = _cutsceneTriggerRoot.GetComponentsWithDepth<CutscenePreset>(1);
            var datas = new CutsceneTriggerData[directors.Count];

            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = directors[i].ToData();
            }

            return datas;
        }
    }
}
