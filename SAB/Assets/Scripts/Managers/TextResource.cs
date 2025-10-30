using Chu.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;

public class TextResource<T> : Singleton<TextResource<T>>
{
    private static Dictionary<T, string> _texts;

    public static string Texts(T type)
    {
        return _texts[type];
    }

    public string this[T index]
    {
        get => _texts[index];
    }

    public void LoadTexts(string type)
    {
        var json = AssetManager.ExternalFolder.GetLanguagesTextFile(type);
        _texts = JsonConvert.DeserializeObject<Dictionary<T, string>>(json);
    }
}
