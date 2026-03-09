using System;
using UnityEditor;

[Serializable]
public class ExcelGeneratePath
{
    public DefaultAsset DataDTO;
    public DefaultAsset Localization;
    public DefaultAsset Enum;

    public string GetPath(SheetType type)
    {
        if (type == SheetType.Data)
            return AssetDatabase.GetAssetPath(DataDTO);
        else if (type == SheetType.Localization)
            return AssetDatabase.GetAssetPath(Localization);
        else /*(type == SheetType.Enum)*/
            return AssetDatabase.GetAssetPath(Enum);
    }
}
