using Chu.Collision;
using Newtonsoft.Json;
using UnityEngine;

namespace SAB.Cutscene
{
    public class CutsceneTriggerData
    {
        public string Name;
        public float CenterX;
        public float CenterY;
        public ShapeParam[] TriggerZones;

        [JsonIgnore]
        public Vector2 Center
        {
            get => new(CenterX, CenterY);
        }
    }
}
