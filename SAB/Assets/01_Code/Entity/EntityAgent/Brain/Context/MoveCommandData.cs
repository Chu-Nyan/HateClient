using UnityEngine;

namespace SAB.EntityAgent.AI.Context
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
