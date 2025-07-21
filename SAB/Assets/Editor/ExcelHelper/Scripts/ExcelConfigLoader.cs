using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ExcelConfigLoader
{
    private const string _sheetPropertyName = "SheetProperty";
    private const string _convertSettingName = "ConvertSetting";

    public ConvertSetting LoadConvertSetting(DataTableCollection table)
    {
        var sheet = table[_convertSettingName];
        var _convertSetting = new ConvertSetting();
        var type = _convertSetting.GetType();
        var nameColumn = 0;
        var valueColumn = 1;

        for (int i = 1; i < sheet.Rows.Count; i++)
        {
            var fieldName = sheet.Rows[i][nameColumn].ToString();
            if (ExcelHelper.HasIgnoreSymbol(fieldName) == true)
                continue;

            var fieldInfo = type.GetField(fieldName);
            if (fieldInfo == null)
                Debug.Log($"{fieldInfo} 누락");
            else
            {
                var value = Convert.ChangeType(sheet.Rows[i][valueColumn].ToString(), fieldInfo.FieldType);
                fieldInfo.SetValue(_convertSetting, value);
            }
        }

        _convertSetting.SetZeroBase();
        return _convertSetting;
    }

    public Dictionary<string, SheetData> LoadSheetProperty(DataTableCollection table, ConvertSetting setting)
    {
        var _sheetDatas = new Dictionary<string, SheetData>();
        var option = table[_sheetPropertyName];
        var firstPropertyColumn = setting.SheetPropertyNameColumn + 1;

        for (int x = setting.SheetPropertyFirstDataRow; x < option.Rows.Count; x++)
        {
            var name = option.Rows[x][setting.SheetPropertyNameColumn].ToString();
            if (table[name] == null)
                continue;

            var sheetData = new SheetData(table[name]);
            var flag = 0;

            for (int y = firstPropertyColumn; y < option.Columns.Count; y++)
            {
                var columnName = option.Rows[0][y].ToString(); // 0 = 열 종류 이름
                var cellText = option.Rows[x][y].ToString();
                if (Enum.TryParse<SheetProperty>(cellText, out var result) == true)
                    flag += (int)result;
                else if (columnName == setting.SheetPropertyGeneratePathName)
                    sheetData.SetPathFromAssetFolder(cellText);
            }

            sheetData.Options = flag;
            if (_sheetDatas.TryAdd(name, sheetData) == false)
                Debug.LogError($"{name} 시트 중복");
        }

        return _sheetDatas;
    }
}
