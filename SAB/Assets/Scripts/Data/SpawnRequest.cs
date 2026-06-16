using SAB.Cutscene;
using UnityEngine;

namespace SAB.Unit
{
    public class SpawnRequest : IObjectConfig
    {
        public int ID;
        public Vector3 Position;
        public Quaternion Rotation;

        public SpawnRequest(int id, Vector3 position, Quaternion rotation)
        {
            ID = id;
            Position = position;
            Rotation = rotation;
        }
    }
}
