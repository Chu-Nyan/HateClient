using SAB.Cutscene;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.DataManger
{
    public class CutSceneRepository
    {
        public readonly Dictionary<MapType, CutsceneData[]> DataByMapType;
        public const string FileNameFormat = "{0}_Cutscene";

        public CutSceneRepository()
        {
            DataByMapType = new();
            foreach (MapType type in Enum.GetValues(typeof(MapType)))
            {
                CutsceneData[] data = AssetManager.DeserializeJsonSync<CutsceneData[]>(string.Format(FileNameFormat, type.ToString()));
                if (data == null)
                {
                    Debug.LogWarning($"{type} CutScene Data can't find");
                }
                else
                {
                    DataByMapType.Add(type, data);
                }
            }
        }
    }
}
