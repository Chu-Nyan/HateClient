using Chu.Core;
using SAB.Cutscene;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class CutsceneRepository
    {
        public readonly Dictionary<string, CutsceneData> DataByName;

        public CutsceneRepository()
        {
            DataByName = new();
            foreach (MapType type in Enum.GetValues(typeof(MapType)))
            {
                string json = AssetManager.LoadJson(string.Format(Const.Asset_DB_Cutscene, type.ToString()));
                CutsceneDataDto[] dataDtos = DataBase.ConvertJsonToArray<CutsceneDataDto>(json);
                if (dataDtos == null)
                    throw new Exception($"{type} CutScene Data can't find");

                foreach (var item in dataDtos)
                {
                    DataByName[item.Name] = CutsceneData.FromDTO(item);
                }
            }
        }
    }
}
