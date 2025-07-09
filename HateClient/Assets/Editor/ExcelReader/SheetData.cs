using System.Data;

public class SheetData
{
    public DataTable Table;
    public int Options;

    public SheetData(DataTable table, int options)
    {
        Table = table;
        Options = options;
    }

    public bool HasFlag(ExcelReadConvertType type)
    {
        return (Options & (int)type) == (int)type;
    }

    public string GetNameFromOptions()
    {
        var suffix = string.Empty;
        if (HasFlag(ExcelReadConvertType.DTO) == true)
            suffix += "DTO";

        return $"{Table.TableName}{suffix}";
    }
}
