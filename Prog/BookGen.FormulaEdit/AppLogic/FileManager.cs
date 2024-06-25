using System.Collections.Generic;
using System.IO;

namespace BookGen.FormulaEdit.AppLogic;

internal static class FileManager
{
    private const string LineStart = "\\";

    public static IEnumerable<string> LoadFile(string path)
    {
        string? line;
        using var reader = File.OpenText(path);
        while ((line = reader.ReadLine()) != null)
        {
            if (!string.IsNullOrWhiteSpace(line) && line.StartsWith(LineStart))
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
            if (line.StartsWith(LineStart))
            {
                writer.WriteLine(line);
            }
            else
            {
                writer.Write(LineStart);
                writer.WriteLine(line);
            }
        }
    }
}
