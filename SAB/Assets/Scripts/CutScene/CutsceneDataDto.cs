using Chu.Collision;
using Chu.Utility;
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

        public Dictionary<int, BindingSource> BindingSourceByID;
        public Dictionary<int, UniqueEntityType> BindingSlots;
        public Dictionary<int, int> SceneObjectBindingIDs;
        public Dictionary<int, VCamStaticData> StaticData;
        public Dictionary<int, VCamFollowData> FollowData;
        public Dictionary<int, SpawnRequest> CharacterData;
        public Dictionary<int, SingleMeshData> SingleMeshData;

        public CutsceneDataDto(string name, string assetPath, Vector2 center, ShapeParam[] triggerZones, Dictionary<string, int> idByTrack)
        {
            BindingSourceByID = new();
            BindingSlots = new();
            SceneObjectBindingIDs = new();
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
            var data = new CutsceneData()
            {
                Name = Name,
                AssetPath = AssetPath,
                CenterX = CenterX,
                CenterY = CenterY,
                TriggerZones = TriggerZones,
                BindingIDByTrack = BindingIDByTrack,
                BindingSourceByID = BindingSourceByID,
                BindingSlots = BindingSlots,
                SceneObjectBindingIDs = SceneObjectBindingIDs,
                ObjectDataContainer = GetObjectDataContainer()
            };

            return data;
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

        public void AddObject(int id, GameObject obj, CutsceneJsonConverter idHandler)
        {
            if (obj.TryGetComponent<CutsceneObject>(out var cutsceneObj) == true)
            {
                BindingSourceByID.Add(id, cutsceneObj.BindingSource);

                if (cutsceneObj.BindingSource == BindingSource.Slot)
                    BindingSlots.Add(id, cutsceneObj.BindingSlot);
                else if (cutsceneObj.BindingSource == BindingSource.Spawn)
                    AddObjectData(id, cutsceneObj, idHandler);
            }
            else // 씬 오브젝트
            {
                BindingSourceByID.Add(id, BindingSource.SceneObject);
                SceneObjectBindingIDs.Add(id, obj.name.GetHashCode());
            }
        }

        private void AddObjectData(int id, CutsceneObject obj, CutsceneJsonConverter idHandler)
        {
            switch (obj.GetCutsceneObjectData(idHandler))
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

        public void AddTrackData(string trackName, int objID)
        {
            if (BindingIDByTrack.TryAdd(trackName, objID) == false
                && BindingIDByTrack[trackName] != objID)
            {
                throw new Exception(string.Format(ErrorMessages.DuplicateKeyMismatched, trackName, BindingIDByTrack[trackName], objID));
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
