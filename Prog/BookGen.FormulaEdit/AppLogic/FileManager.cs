using System.Collections.Generic;
using System.IO;

namespace BookGen.FormulaEdit.AppLogic;

internal static class FileManager
{
    public static IEnumerable<string> LoadFile(string path)
    {
        string? line;
        using var reader = File.OpenText(path);
        while ((line = reader.ReadLine()) != null)
        {
            if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("\\"))
            {
                yield return line;
            }
        }
    }

    public static void SaveFile(string path, IEnumerable<string> lines)
    {
        using var writer = File.CreateText(path);
        foreach (var line in lines)
        {
            if (line.StartsWith("\\"))
            {
                writer.WriteLine(line);
            }
            else
            {
                writer.Write("\\");
                writer.WriteLine(line);
            }
        }
    }
}
