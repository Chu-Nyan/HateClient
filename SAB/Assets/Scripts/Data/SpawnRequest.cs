using SAB.Cutscene;
using SAB.EntityAgent;
using UnityEngine;

namespace SAB.Unit
{
    public class SpawnRequest : ICutscenePreset
    {
        public int UnitID;
        public BrainType BrainType;
        public Vector3 Position;
        public Quaternion Rotation;

        public SpawnRequest(int unitId, BrainType brainType, Vector3 position, Quaternion rotation)
        {
            UnitID = unitId;
            BrainType = brainType;
            Position = position;
            Rotation = rotation;
        }
    }
}
