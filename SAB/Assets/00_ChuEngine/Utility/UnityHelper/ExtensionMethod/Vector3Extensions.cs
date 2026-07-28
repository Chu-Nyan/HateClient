using UnityEngine;

namespace Chu.Utility.Unity
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Vector3(X, Y, Z)를 Vector2(X, Z)로 변환합니다.
        /// </summary>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static Vector2 ToVector2XZ(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }
    }

    public static class Vector2Extensions
    {
        public static Vector3 ToVector3XZ(this Vector2 v2)
        {
            return new Vector3(v2.x, 0, v2.y);
        }
    }
}
