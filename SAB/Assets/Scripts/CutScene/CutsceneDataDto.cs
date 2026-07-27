using Chu.Utility;
using SAB.GameSystem;
using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneDataDto
    {
        public string Name;
        public bool LockPlayer;

        public Dictionary<string, int> BindingIDByTrack;
        public Dictionary<int, BindingSource> BindingSourceById;
        public Dictionary<int, UniqueEntityType> UniqueSlotById;
        public Dictionary<int, string> FavoritesById;
        public Dictionary<int, VCamStaticData> StaticData;
        public Dictionary<int, VCamFollowData> FollowData;
        public Dictionary<int, CharacterSpawnRequest> CharacterData;
        public Dictionary<int, SingleMeshData> SingleMeshData;
        public HashSet<int> PersistentObjects;

        public CutsceneDataDto(string name, bool lockPlayer, Dictionary<string, int> idByTrack)
        {
            Name = name;
            LockPlayer = lockPlayer;
            BindingSourceById = new();
            UniqueSlotById = new();
            FavoritesById = new();
            StaticData = new();
            FollowData = new();
            CharacterData = new();
            SingleMeshData = new();
            PersistentObjects = new();
            BindingIDByTrack = idByTrack;
        }

        public void AddObject(int id, GameObject obj, CutsceneJsonConverter idHandler)
        {
            if (BindingSourceById.ContainsKey(id) == true)
                return;

            if (obj.TryGetComponent<CutsceneObjectPreset>(out var cutsceneObj) == true)
            {
                BindingSourceById.Add(id, cutsceneObj.BindingSource);
                if (cutsceneObj.IsPersistent == true)
                    PersistentObjects.Add(id);

                if (cutsceneObj.BindingSource == BindingSource.Slot)
                    UniqueSlotById.Add(id, (UniqueEntityType)cutsceneObj.VariantParams[CutsceneObjectPreset.BindingSlot].IntValue);
                else if (cutsceneObj.BindingSource == BindingSource.Spawn)
                    AddObjectData(id, cutsceneObj, idHandler);
            }
            else if (obj.TryGetComponent<ObjectMarker>(out var marker))// 씬 오브젝트
            {
                BindingSourceById.Add(id, BindingSource.SceneObject);
                FavoritesById.Add(id, marker.FavoriteID);
            }
            else
            {
                throw new Exception($"Serialization Error:  Failed to find serialization component"
                    + $"Object Name : {obj.name}");
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
                case CharacterSpawnRequest character:
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
