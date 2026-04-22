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
        public ShapeParam[] TriggerZones;
        public Dictionary<string, int> BindingIDByTrack;
        public Dictionary<int, SingleMeshData> SingleMeshDataByID;
        public List<CutsceneVCamData> VCamData;

        [JsonIgnore]
        public Vector2 Center
        {
            get => new Vector2(CenterX, CenterY);
        }

        public CutsceneData(string name, string assetPath, Vector2 center, ShapeParam[] triggerZones, Dictionary<string, int> idByTrack, Dictionary<int, SingleMeshData> singleMesh, List<CutsceneVCamData> vcamDatas)
        {
            Name = name;
            AssetPath = assetPath;
            CenterX = center.x;
            CenterY = center.y;
            TriggerZones = triggerZones;
            BindingIDByTrack = idByTrack;
            SingleMeshDataByID = singleMesh;
            VCamData = vcamDatas;
        }
    }
}
