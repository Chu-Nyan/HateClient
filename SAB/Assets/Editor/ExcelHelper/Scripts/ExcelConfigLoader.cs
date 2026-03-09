using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ExcelConfigLoader
{
    private const string _sheetPropertyName = "SheetProperty";

    public Dictionary<string, SheetData> LoadSheetProperty(DataTableCollection table, ConvertSetting setting, ExcelGeneratePath path)
    {
        var _sheetDatas = new Dictionary<string, SheetData>();
        var option = table[_sheetPropertyName];

        for (int x = setting.SheetPropertyFirstDataRow; x < option.Rows.Count; x++)
        {
            var name = option.Rows[x][setting.SheetPropertyNameColumn].ToString();
            if (table[name] == null)
                throw new Exception("존재 하지 않는 시트 이름");

            var typeText = option.Rows[x][setting.SheetPropertyTypeColumn].ToString();
            if (Enum.TryParse(typeText, out SheetType type) == false)
                throw new Exception("변환 실패");

            var sheetData = new SheetData(table[name], type, path.GetPath(type));
            if (_sheetDatas.TryAdd(name, sheetData) == false)
                throw new Exception($"{name} 시트 중복");
        }

        return _sheetDatas;
    }
}
