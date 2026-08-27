using SAB.GameSystem;
using SAB.Unit;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class CutsceneData
    {
        public string Name;
        public bool LockPlayer;

        public Dictionary<string, int> BindingIDByTrack;
        public Dictionary<int, BindingSource> BindingSourceByID;
        public Dictionary<int, UniqueEntityType> UniqueSlotByID;
        public Dictionary<int, string> FavoritesByID;
        public Dictionary<int, VCamStaticData> StaticData;
        public Dictionary<int, VCamFollowData> FollowData;
        public Dictionary<int, CharacterSpawnRequest> CharacterData;
        public HashSet<int> PersistentObjects;
    }
}
