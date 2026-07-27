using Chu.Core;
using Newtonsoft.Json;
using SAB.GameSystem;
using System;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class MapRepository
    {
        public readonly Dictionary<MapType, MapDefinition> DataByType;

        public MapRepository()
        {
            DataByType = new();

            foreach (MapType type in Enum.GetValues(typeof(MapType)))
            {
                DataByType[type] = new();

                string json = AssetManager.LoadJson(string.Format(Const.Asset_DB_MapGeneratedData, type.ToString()));
                MapDefinition dataDtos = JsonConvert.DeserializeObject<MapDefinition>(json, DataBase.JsonSerializerSettings);

                if (dataDtos == null)
                    throw new Exception($"{type} CutScene Data can't find");

                DataByType[type] = dataDtos;
            }
        }
    }
}
