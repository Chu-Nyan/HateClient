using Chu.Collision;
using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneDataDto
    {
        public string Name;
        public string AssetPath;
        public float CenterX;
        public float CenterY;
        public ShapeParam[] TriggerZones;
        public Dictionary<string, int> BindingIDByTrack;

        public Dictionary<int, VCamStaticData> StaticData;
        public Dictionary<int, VCamFollowData> FollowData;
        public Dictionary<int, SpawnRequest> CharacterData;
        public Dictionary<int, SingleMeshData> SingleMeshData;

        public CutsceneDataDto(string name, string assetPath, Vector2 center, ShapeParam[] triggerZones, Dictionary<string, int> idByTrack)
        {
            StaticData = new();
            FollowData = new();
            CharacterData = new();
            SingleMeshData = new();

            Name = name;
            AssetPath = assetPath;
            CenterX = center.x;
            CenterY = center.y;
            TriggerZones = triggerZones;
            BindingIDByTrack = idByTrack;
        }

        public CutsceneData GetContainer()
        {
            return new CutsceneData(Name, AssetPath, new(CenterX, CenterY), TriggerZones, BindingIDByTrack, GetObjectDataContainer());
        }

        private ObjectDataContainer GetObjectDataContainer()
        {
            var container = new ObjectDataContainer();
            foreach (var item in StaticData)
            {
                container.Add(item.Key, item.Value);
            }
            foreach (var item in FollowData)
            {
                container.Add(item.Key, item.Value);
            }
            foreach (var item in CharacterData)
            {
                container.Add(item.Key, item.Value);
            }
            foreach (var item in SingleMeshData)
            {
                container.Add(item.Key, item.Value);
            }

            return container;
        }

        public void AddObjectData(int id, IObjectConfig data)
        {
            switch (data)
            {
                case VCamStaticData staticData:
                    AddDataArray(StaticData, id, staticData);
                    break;
                case VCamFollowData follow:
                    AddDataArray(FollowData, id, follow);
                    break;
                case SpawnRequest character:
                    AddDataArray(CharacterData, id, character);
                    break;
                case SingleMeshData mesh:
                    AddDataArray(SingleMeshData, id, mesh);
                    break;
            }
        }

        private void AddDataArray<T, K>(T arr, int id, K value) where T : IDictionary<int, K> where K : IObjectConfig
        {
            if (arr.TryAdd(id, value) == true)
            {
                if (arr[id].Equals(value) == false)
                {
                    throw new Exception($"Object ID : {id}, Type : {typeof(K).Name} : Duplicate object name found");
                }
            }
        }
    }
}
