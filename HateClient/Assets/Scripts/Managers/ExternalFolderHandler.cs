using System.IO;
using UnityEngine;

public class ExternalFolderHandler
{
#if UNITY_EDITOR
    public readonly string ExternalFolder = Path.Combine(Application.dataPath, "Editor", ExternalFolderName);
#elif UNITY_IOS || UNITY_ANDROID
    public readonly string ExternalFolder = Application.persistentDataPath;
#else
    public readonly string ExternalFolder = Path.Combine(Directory.GetParent(Application.dataPath)?.FullName, ExternalFolderName);
#endif
    private const string ExternalFolderName = "External_Data";
    private const string LanguagesFolder = "Languages";

    //TODO
    public void ChangeFont()
    {

    }

    public string GetLanguagesTextFile(string name)
    {
        name = Path.Combine(ExternalFolder, $"{name}.json");
        return System.IO.File.ReadAllText(name);
    }
}
