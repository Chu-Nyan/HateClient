using System.Collections.Generic;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Chu.Utility.Unity
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

        public static string GetAddressablePath(UnityEngine.Object obj)
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(obj);
            string guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings.FindAssetEntry(guid);

            if (entry != null)
                return entry.address;
            else
                throw new System.Exception($"{obj.name}, No Addressable Asset");
        }
    }
}
