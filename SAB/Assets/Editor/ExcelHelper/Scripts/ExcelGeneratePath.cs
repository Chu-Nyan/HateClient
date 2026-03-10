using System;
using UnityEditor;

[Serializable]
public class ExcelGeneratePath
{
    public DefaultAsset DataDTO;
    public DefaultAsset Enum;

    public string GetPath(SheetType type)
    {
        if (type == SheetType.Data)
            return AssetDatabase.GetAssetPath(DataDTO);
        else /*(type == SheetType.Enum)*/
            return AssetDatabase.GetAssetPath(Enum);
    }
}
