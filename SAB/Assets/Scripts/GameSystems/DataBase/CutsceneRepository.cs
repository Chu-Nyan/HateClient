using Chu.Core;
using SAB.Cutscene;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class CutsceneRepository
    {
        public readonly Dictionary<string, CutsceneData> DataByName;
        public readonly Dictionary<MapType, string[]> CutsceneNameByMapType;
        public const string FileNameFormat = "{0}_Cutscene";

        public CutsceneRepository()
        {
            CutsceneNameByMapType = new();
            DataByName = new();
            foreach (MapType type in Enum.GetValues(typeof(MapType)))
            {
                string json = AssetManager.LoadJson(string.Format(FileNameFormat, type.ToString()));
                CutsceneDataDto[] dataDtos = DataBase.ConvertJsonToArray<CutsceneDataDto>(json);
                if (dataDtos == null)
                    throw new Exception($"{type} CutScene Data can't find");

                CutsceneNameByMapType.Add(type, new string[dataDtos.Length]);
                for (int i = 0; i < dataDtos.Length; i++)
                {
                    var data = dataDtos[i].GetContainer();
                    DataByName.Add(data.Name, data);
                    CutsceneNameByMapType[type][i] = data.Name;
                }
            }
        }
    }
}
