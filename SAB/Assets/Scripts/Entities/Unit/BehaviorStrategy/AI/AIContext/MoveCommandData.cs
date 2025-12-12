using UnityEngine;

namespace SAB.AI.Data
{
    public struct MoveCommandData
    {
        public Vector3 Destination;

        public MoveCommandData(Vector3 destination)
        {
            Destination = destination;
        }
    }
}
