using UnityEngine;

namespace Chu.Collision
{
    public struct CircleRangeData
    {
        public Vector3 Offset;
        public float Radius;

        public CircleRangeData(Vector3 offset, float radius)
        {
            Offset = offset;
            Radius = radius;
        }
    }
}