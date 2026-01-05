using System.Data;
using System.IO;
using UnityEngine;

public class SheetData
{
    private static readonly string _assetPath = Application.dataPath;

    private readonly DataTable _table;
    private int _option;
    private string _generatePath;

    public DataTable Table
    {
        get => _table;
    }

    public int Options
    {
        get => _option;
        set => _option = value;
    }

    public string GeneratePath
    {
        get => _generatePath;
    }

    public SheetData(DataTable table)
    {
        _table = table;
    }

    public void SetPathFromAssetFolder(string path)
    {
        _generatePath = Path.Combine(_assetPath, path);
    }

    public bool HasFlag(SheetProperty type)
    {
        return (Options & (int)type) == (int)type;
    }

    public string GetNameFromOptions()
    {
        var suffix = string.Empty;
        if (HasFlag(SheetProperty.Data) == true)
            suffix += "_DTO";

        return $"{Table.TableName}{suffix}";
    }
}
