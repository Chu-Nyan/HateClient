using SAB.EntityAgent;
using UnityEngine;

namespace SAB.Unit
{
    public class CharacterSpawnRequest
    {
        public int UnitID;
        public BrainType BrainType;
        public Vector3 Position;
        public Quaternion Rotation;

        public CharacterSpawnRequest(int unitId, BrainType brainType, Vector3 position, Quaternion rotation)
        {
            UnitID = unitId;
            BrainType = brainType;
            Position = position;
            Rotation = rotation;
        }
    }
}
