using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Chu.Utility.UnityHelper
{
    public static class Utility
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

        public static void GenerateFile(string path, string fileName, string text)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            File.WriteAllText(Path.Combine(path, fileName), text);
        }
    }
}
