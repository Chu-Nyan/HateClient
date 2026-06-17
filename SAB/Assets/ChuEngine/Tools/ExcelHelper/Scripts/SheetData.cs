using System.Data;

namespace Chu.Tools
{
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

        public string SheetName
        {
            get => _table.TableName;
        }

        public SheetData(DataTable table, SheetType type)
        {
            _table = table;
            _type = type;
        }

        public string GetPacalCaseName()
        {
            string cleanName = Table.TableName
                .Replace("_", "")
                .Replace("-", "")
                .Replace(" ", "");

            return cleanName;
        }
    }
}
