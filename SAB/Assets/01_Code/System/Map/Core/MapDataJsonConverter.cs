using Chu.Data;
using Chu.Utility;
using Chu.Utility.Json;
using Newtonsoft.Json;
using SAB.Cutscene;
using SAB.Unit;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SAB.GameSystem
{
    public class MapDataJsonConverter : MonoBehaviour
    {
        private const string MapPath = "04_Data/DB/Map";
        private const string CutscenePath = "04_Data/DB/Cutscene";

        [SerializeField]
        private MapType _type;
        [SerializeField]
        private CutsceneDataExtractor _cutsceneConverter;
        [SerializeField]
        private MapReferenceContainer _referenceJsonConverter;

        [ContextMenu("Convert")]
        public void Convert()
        {
            ConvertCutscene();
            ConvertMap();
            Debug.Log("Data Exported");
        }

        private void ConvertMap()
        {
            int id = 0;
            _referenceJsonConverter.AutoBinding();

            var data = new MapDefinition
            {
                Characters = new Dictionary<int, CharacterSpawnRequest>(),
                SpawnPoint = new Pose2D[_referenceJsonConverter.SpawnPoint.Length],
                CutsceneTriggers = _cutsceneConverter.GetTriggerData(),
                Favorites = _referenceJsonConverter.FavoriteObjects
            };

            foreach (var item in _referenceJsonConverter.Characters)
            {
                var acter = item.Value;
                var spwanData = new CharacterSpawnRequest(acter.Stats.CharacterID, acter.BrainType, acter.transform.position, acter.transform.rotation);
                data.Characters.Add(++id, spwanData);
            }

            for (int i = 0; i < _referenceJsonConverter.SpawnPoint.Length; i++)
            {
                var pos2D = _referenceJsonConverter.SpawnPoint[i].position.ToVector2XZ();
                var y = _referenceJsonConverter.SpawnPoint[i].rotation.y;
                data.SpawnPoint[i] = new(pos2D, y);
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings().WithUnity());
            string path = Path.Combine(Application.dataPath, MapPath);
            FileUtility.GenerateFile(path, $"{string.Format(Const.Asset_DB_MapGenerated, _type)}.json", json);
        }

        private void ConvertCutscene()
        {
            var data = _cutsceneConverter.GetCutsceneData();
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings().WithUnity());
            string path = Path.Combine(Application.dataPath, CutscenePath);
            FileUtility.GenerateFile(path, $"{string.Format(Const.Asset_DB_Cutscene, _type)}.json", json);
        }
    }
}
