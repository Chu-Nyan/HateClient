using UnityEngine;

namespace Chu.Collision
{
    public struct CircleRangeData
    {
        public static CircleRangeData Default = new(Vector3.zero, 1);

        public Vector3 Offset;
        public float Radius;

        public CircleRangeData(Vector3 offset, float radius)
        {
            Offset = offset;
            Radius = radius;
        }
    }
}