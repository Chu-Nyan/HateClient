using Chu.Utility;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace SAB.GameSystem
{
    public abstract class DataExporter : MonoBehaviour
    {
        protected abstract string DataPath { get; }
        protected abstract object GenerateData();

        public void Export()
        {
            string json = JsonConvert.SerializeObject(GenerateData(), Formatting.Indented, new JsonSerializerSettings().WithUnity());
            File.WriteAllText($"{DataPath}.json", json);
        }
    }
}
