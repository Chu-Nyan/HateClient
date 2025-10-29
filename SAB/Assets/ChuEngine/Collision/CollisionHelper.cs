using System;
using UnityEngine;

namespace Chu.Collision
{
    /// <summary>
    /// 도형 충돌 여부를 계산하는 유틸리티
    /// </summary>
    public static class CollisionHelper
    {
        public static bool IsColliding(RectShape a, RectShape b)
        {
            Vector2 distance = a.RectBound.Center - b.RectBound.Center;
            bool result = IsOverlapOnAxis(a.Axis[0]) && IsOverlapOnAxis(b.Axis[0])
                       && IsOverlapOnAxis(a.Axis[1]) && IsOverlapOnAxis(b.Axis[1]);

            return result;

            bool IsOverlapOnAxis(Vector2 axis)
            {
                float dotDistance = Mathf.Abs(Vector2.Dot(axis, distance));
                float dotThis = Mathf.Abs(Vector2.Dot(axis, a.Radius[0])) + Mathf.Abs(Vector2.Dot(axis, a.Radius[1]));
                float dotTarget = Mathf.Abs(Vector2.Dot(axis, b.Radius[0])) + Mathf.Abs(Vector2.Dot(axis, sb.Radius[1]));

                return dotDistance <= dotThis + dotTarget;
            }
        }

        public static bool IsColliding(RectShape rect, CircleShape circle)
        {
            Vector2 distance = rect.RectBound.Center - circle.RectBound.Center;
            float localX = Vector2.Dot(distance, rect.Axis[0]);
            float localY = Vector2.Dot(distance, rect.Axis[1]);

            float closestX = Mathf.Clamp(localX, -rect.RectBound.HalfX, rect.RectBound.HalfX);
            float closestY = Mathf.Clamp(localY, -rect.RectBound.HalfY, rect.RectBound.HalfY);

            float x = localX - closestX;
            float y = localY - closestY;

            return x * x + y * y <= circle.Radius * circle.Radius;
        }

        public static bool IsColliding(CircleShape a, CircleShape b)
        {
            Vector2 aCenter = a.RectBound.Center;
            Vector2 bCenter = b.RectBound.Center;
            float aNum = aCenter.x - bCenter.x;
            float bNum = aCenter.y - bCenter.y;
            float distance = (float)Math.Sqrt(aNum * aNum + bNum * bNum);

            return distance <= (a.Radius + b.Radius);
        }
    }
}
