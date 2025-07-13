public class ConvertSetting
{
    public int DBNameRow;
    public int DBTypeRow;
    public int DBDataStartedRow;

    public int EnumDataStartedRow;
    public int EnumTypeColumn;
    public int EnumKeyColumn;
    public int EnumValueColumn;
    public int EnumCommentsColumn;

    public int LocalizationNameRow;
    public int LocalizationKeyColumn;
    public int LocalizationFirstDataRow;

    public int SheetPropertyNameColumn;
    public int SheetPropertyFirstDataRow;
    public string SheetPropertyGeneratePathName;

    private bool _isZeroBase;

    public void SetZeroBase()
    {
        if (_isZeroBase == true)
            return;

        _isZeroBase = true;

        DBNameRow--;
        DBTypeRow--;
        DBDataStartedRow--;

        EnumDataStartedRow--;
        EnumTypeColumn--;
        EnumKeyColumn--;
        EnumValueColumn--;
        EnumCommentsColumn--;

        LocalizationKeyColumn--;
        LocalizationNameRow--;
        LocalizationFirstDataRow--;

        SheetPropertyNameColumn--;
        SheetPropertyFirstDataRow--;
    }
}
