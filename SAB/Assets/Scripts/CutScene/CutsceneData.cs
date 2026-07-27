using SAB.GameSystem;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class CutsceneData
    {
        public string Name;
        public bool LockPlayer;

        public Dictionary<string, int> BindingIDByTrack;
        public Dictionary<int, BindingSource> BindingSourceByID;
        public Dictionary<int, UniqueEntityType> BindingSlots;
        public Dictionary<int, string> SceneObjectBindingIDs;
        public SpawnDataContainer SpawnContainer;
        public HashSet<int> PersistentObjects;

        public static CutsceneData FromDTO(CutsceneDataDto dto)
        {
            var container = new SpawnDataContainer();
            foreach (var item in dto.StaticData)
            {
                container.Add(item.Key, item.Value, false);
            }
            foreach (var item in dto.FollowData)
            {
                container.Add(item.Key, item.Value, false);
            }
            foreach (var item in dto.CharacterData)
            {
                container.Add(item.Key, item.Value, true);
            }
            foreach (var item in dto.SingleMeshData)
            {
                container.Add(item.Key, item.Value, false);
            }

            return new CutsceneData()
            {
                Name = dto.Name,
                LockPlayer = dto.LockPlayer,
                BindingIDByTrack = dto.BindingIDByTrack,
                BindingSourceByID = dto.BindingSourceById,
                BindingSlots = dto.UniqueSlotById,
                SceneObjectBindingIDs = dto.FavoritesById,
                PersistentObjects = dto.PersistentObjects,
                SpawnContainer = container
            };
        }
    }
}
