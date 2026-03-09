using System.Data;

public class SheetData
{
    private readonly DataTable _table;
    private SheetType _type;
    private string _generatePath;

    public DataTable Table
    {
        get => _table;
    }

    public SheetType Type
    {
        get => _type;
    }

    public string GeneratePath
    {
        get => _generatePath;
    }

    public SheetData(DataTable table, SheetType type, string path)
    {
        _table = table;
        _type = type;
        _generatePath = path;
    }

    public string GetNameFromOptions()
    {
        var suffix = string.Empty;
        if (_type == SheetType.Data)
            suffix += "_DTO";

        return $"{Table.TableName}{suffix}";
    }
}
