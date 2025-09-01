using System.Runtime.CompilerServices;
using UnityEngine;

namespace Chu.Data
{
    public struct RectBound
    {
        public float MinX, MinY, MaxX, MaxY;
        public float HalfX, HalfY;

        public readonly Vector2 Center
        {
            get => new(MinX + HalfX, MinY + HalfY);
        }

        public RectBound(float minX, float maxX, float minY, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;

            HalfX = (maxX - minX) * 0.5f;
            HalfY = (maxY - minY) * 0.5f;
        }

        public RectBound(float minX, float maxX, float minY, float maxY, float halfX, float halfY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;

            HalfX = halfX;
            HalfY = halfY;
        }

        public void RefreshPosition(Vector2 pos)
        {
            MinX = pos.x - HalfX;
            MaxX = pos.x + HalfX;
            MinY = pos.y - HalfY;
            MaxY = pos.y + HalfY;
        }

        // 회전 상태를 반영한 크기 반환
        public readonly RectBound GetWorldCorners(Vector2[] Radius)
        {
            Vector2 center = Center;
            Vector2 r0 = Radius[0];
            Vector2 r1 = Radius[1];

            float minX = center.x - Mathf.Abs(r0.x) - Mathf.Abs(r1.x);
            float maxX = center.x + Mathf.Abs(r0.x) + Mathf.Abs(r1.x);
            float minY = center.y - Mathf.Abs(r0.y) - Mathf.Abs(r1.y);
            float maxY = center.y + Mathf.Abs(r0.y) + Mathf.Abs(r1.y);

            return new RectBound(minX, maxX, minY, maxY);
        }


        // 내부에 target이 완벽하게 포함되는가?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsFullyInside(RectBound target)
        {
            return MinX <= target.MinX && MaxX >= target.MaxX
                && MinY <= target.MinY && MaxY >= target.MaxY;
        }

        // 두 RectBound는 겹치는가?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsIntersecting(RectBound a, RectBound b)
        {
            if (a.MaxX < b.MinX) return false;
            if (a.MinX > b.MaxX) return false;
            if (a.MaxY < b.MinY) return false;
            if (a.MinY > b.MaxY) return false;

            return true;
        }

        public override readonly string ToString()
        {
            return $"X : {MinX} / {MaxX}, Y :{MinY} / {MaxY}";
        }
    }
}