using System.IO;
using UnityEditor.AddressableAssets;

namespace Chu.Utility
{
    public static class FileUtility
    {
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
