//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BookGen.Cli;

public interface IFileSystem
{
    bool DirectoryExists(string path);
    bool FileExists(string path);
    void WriteAllText(string file, string content);
    Stream CreateStream(string target);
    string ReadAllText(string inputFile);
}

public sealed class FileSystem : IFileSystem
{
    public bool DirectoryExists(string path)
        => Directory.Exists(path);

    public bool FileExists(string path)
        => File.Exists(path);

    public void WriteAllText(string file, string content)
        => File.WriteAllText(file, content);

    public Stream CreateStream(string target)
    {
        string? dir = Path.GetDirectoryName(target) ?? string.Empty;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return File.Create(target);
    }

    public string ReadAllText(string inputFile)
        => File.ReadAllText(inputFile);
}