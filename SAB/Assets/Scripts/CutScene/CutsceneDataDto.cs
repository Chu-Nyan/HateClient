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
        public bool LockPlayer;
        public ShapeParam[] TriggerZones;
        public Dictionary<string, int> BindingIDByTrack;

        public Dictionary<int, BindingSource> BindingSourceById;
        public Dictionary<int, UniqueEntityType> UniqueSlotById;
        public Dictionary<int, string> FavoritesById;
        public Dictionary<int, VCamStaticData> StaticData;
        public Dictionary<int, VCamFollowData> FollowData;
        public Dictionary<int, SpawnRequest> CharacterData;
        public Dictionary<int, SingleMeshData> SingleMeshData;
        public HashSet<int> PersistentObjects;

        public CutsceneDataDto(string name, string assetPath, Vector2 center, bool lockPlayer, ShapeParam[] triggerZones, Dictionary<string, int> idByTrack)
        {
            BindingSourceById = new();
            UniqueSlotById = new();
            FavoritesById = new();
            StaticData = new();
            FollowData = new();
            CharacterData = new();
            SingleMeshData = new();
            PersistentObjects = new();

            Name = name;
            AssetPath = assetPath;
            CenterX = center.x;
            CenterY = center.y;
            LockPlayer = lockPlayer;
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
                LockPlayer = LockPlayer,
                TriggerZones = TriggerZones,
                BindingIDByTrack = BindingIDByTrack,
                BindingSourceByID = BindingSourceById,
                BindingSlots = UniqueSlotById,
                SceneObjectBindingIDs = FavoritesById,
                PersistentObjects = PersistentObjects,
                SpawnContainer = GetObjectDataContainer()
            };

            return data;
        }

        private SpawnDataContainer GetObjectDataContainer()
        {
            var container = new SpawnDataContainer();
            foreach (var item in StaticData)
            {
                container.Add(item.Key, item.Value, false);
            }
            foreach (var item in FollowData)
            {
                container.Add(item.Key, item.Value, false);
            }
            foreach (var item in CharacterData)
            {
                container.Add(item.Key, item.Value, true);
            }
            foreach (var item in SingleMeshData)
            {
                container.Add(item.Key, item.Value, false);
            }

            return container;
        }

        public void AddObject(int id, GameObject obj, CutsceneJsonConverter idHandler, MapReferenceHub hub)
        {
            if (BindingSourceById.ContainsKey(id) == true)
                return;

            if (obj.TryGetComponent<CutsceneObjectPreset>(out var cutsceneObj) == true)
            {
                BindingSourceById.Add(id, cutsceneObj.BindingSource);
                if (cutsceneObj.IsPersistent == true)
                    PersistentObjects.Add(id);

                if (cutsceneObj.BindingSource == BindingSource.Slot)
                    UniqueSlotById.Add(id, cutsceneObj.GetVariantParam<UniqueEntityType>(CutsceneObjectPreset._bindingSlot));
                else if (cutsceneObj.BindingSource == BindingSource.Spawn)
                    AddObjectData(id, cutsceneObj, idHandler);
            }
            else // 씬 오브젝트
            {
                var comps = obj.GetComponents<MonoBehaviour>();
                foreach (var comp in comps)
                {
                    foreach (var item in hub.FavoriteObjects)
                    {
                        if (item.Key != comp)
                            continue;

                        BindingSourceById.Add(id, BindingSource.SceneObject);
                        FavoritesById.Add(id, item.Value);
                        return;
                    }
                }
                Debug.LogWarning($"{Name}, {id} Scene object not found.");
            }
        }

        private void AddObjectData(int id, CutsceneObjectPreset obj, CutsceneJsonConverter idHandler)
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

        private void AddDataArray<T, K>(T arr, int id, K value) where T : IDictionary<int, K> where K : ICutscenePreset
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
