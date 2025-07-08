using System.IO;
using UnityEngine;

public class ExternalFolderHandler
{
#if UNITY_EDITOR
    public readonly string ExternalFolder = Path.Combine(Application.dataPath, "Editor", "ExternalFolder");
#elif UNITY_IOS || UNITY_ANDROID
    public readonly string ExternalFolder = Application.persistentDataPath;
#else
    public readonly string ExternalFolder = Directory.GetParent(Application.dataPath)?.FullName;
#endif
    private readonly string LanguagesFolder = "Languages";

    public string GetLanguagesTextFile(string name)
    {
        name = Path.Combine(ExternalFolder, LanguagesFolder, $"{name}.json");
        return System.IO.File.ReadAllText(name);
    }
}
