using System.IO;

namespace Chu.Tools
{
    public static class ExcelUtility
    {
        public const char IgnoreSymbol = '#';

        public static void GenerateFile(string path, string fileName, string text)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            File.WriteAllText(Path.Combine(path, fileName), text);
        }

        public static bool HasIgnoreSymbol(string text)
        {
            return text.Length == 0 || text[0] == IgnoreSymbol;
        }
    }
}
