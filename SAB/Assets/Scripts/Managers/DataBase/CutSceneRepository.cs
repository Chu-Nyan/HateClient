using SAB.Cutscene;
using System;
using System.Collections.Generic;

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
                CutsceneDataDto[] dataDtos = DataBase.ConvertJsonToArray<CutsceneDataDto>(json);
                if (dataDtos == null)
                    throw new Exception($"{type} CutScene Data can't find");

                var datas = new CutsceneData[dataDtos.Length];
                for (int i = 0; i < dataDtos.Length; i++)
                {
                    datas[i] = dataDtos[i].GetContainer();
                }

                DataByMapType.Add(type, datas);
            }
        }
    }
}
