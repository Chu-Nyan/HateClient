using System.Data;

public class SheetData
{
    private readonly DataTable _table;
    private SheetType _type;

    public DataTable Table
    {
        get => _table;
    }

    public SheetType Type
    {
        get => _type;
    }

    public SheetData(DataTable table, SheetType type)
    {
        _table = table;
        _type = type;
    }

    public string GetName()
    {
        var suffix = string.Empty;
        if (_type == SheetType.Data)
            suffix += "_DTO";

        return $"{Table.TableName}{suffix}";
    }
}
