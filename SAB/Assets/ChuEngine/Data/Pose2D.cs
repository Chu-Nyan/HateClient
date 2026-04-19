using UnityEngine;

namespace Chu.Data
{
    public struct Pose2D
    {
        public Vector2 Position;
        public float EulerY;

        public Pose2D(Vector2 pos, float eulerY)
        {
            Position = pos;
            EulerY = eulerY;
        }
    }
}
