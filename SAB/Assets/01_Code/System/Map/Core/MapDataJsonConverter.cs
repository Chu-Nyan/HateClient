using Chu.Utility;
using Newtonsoft.Json;
using SAB.Cutscene;
using System.IO;
using UnityEngine;

namespace SAB.GameSystem
{
    public class MapDataJsonConverter : MonoBehaviour
    {
        private const string _mapPath = "04_Data/DB/Map";
        private const string _cutscenePath = "04_Data/DB/Cutscene";

        [SerializeField]
        private MapType _type;
        [SerializeField]
        private CutsceneDataExtractor _cutsceneConverter;
        [SerializeField]
        private MapReferenceBinding _mapReference;

        [ContextMenu("Convert")]
        public void Convert()
        {
            ConvertCutscene();
            ConvertMap();
            Debug.Log("Data Exported");
        }

        private void ConvertMap()
        {
            var data = _mapReference.GetGeneratorData();
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings().WithUnity());
            string path = Path.Combine(Application.dataPath, _mapPath);
            FileUtility.GenerateFile(path, $"{string.Format(Const.Asset_DB_MapGenerated, _type)}.json", json);
        }

        private void ConvertCutscene()
        {
            var data = _cutsceneConverter.GetCutsceneData();
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings().WithUnity());
            string path = Path.Combine(Application.dataPath, _cutscenePath);
            FileUtility.GenerateFile(path, $"{string.Format(Const.Asset_DB_Cutscene, _type)}.json", json);
        }
    }
}
