using Chu.Collision;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneData
    {
        public string Name;
        public string AssetPath;
        public float CenterX;
        public float CenterY;
        public bool LockPlayer;
        public ShapeParam[] TriggerZones;
        public Dictionary<string, int> BindingIDByTrack;
        public Dictionary<int, BindingSource> BindingSourceByID;
        public Dictionary<int, UniqueEntityType> BindingSlots;
        public Dictionary<int, string> SceneObjectBindingIDs;
        public SpawnDataContainer SpawnContainer;
        public HashSet<int> PersistentObjects;

        [JsonIgnore]
        public Vector2 Center
        {
            get => new Vector2(CenterX, CenterY);
        }
    }
}
