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
                string json = AssetManager.LoadJson(string.Format(FileNameFormat, type.ToString()));
                CutsceneData[] data = DataBase.ConvertJsonToArray<CutsceneData>(json);
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
