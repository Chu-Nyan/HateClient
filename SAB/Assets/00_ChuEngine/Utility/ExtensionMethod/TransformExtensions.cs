using Chu.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Utility
{
    public static class TransformExtensions
    {
        public static List<T> GetComponentsWithDepth<T>(this Transform root, int maxDepth, bool includeInactive = false)
        {
            List<T> result = new();
            Traverse(root, 0);
            return result;

            void Traverse(Transform current, int depth)
            {
                if (depth > maxDepth)
                    return;

                if (includeInactive == true || current.gameObject.activeInHierarchy)
                {
                    if (current.TryGetComponent<T>(out var comp))
                        result.Add(comp);
                }

                foreach (Transform child in current)
                {
                    Traverse(child, depth + 1);
                }
            }
        }

        public static Pose2D ToPose2D(this Transform root)
        {
            var pos2D = root.position.ToVector2XZ();
            var y = root.rotation.y;

            return new(pos2D, y);
        }
    }
}
