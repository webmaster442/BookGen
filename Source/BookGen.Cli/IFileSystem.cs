//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public interface IFileSystem
{
    bool DirectoryExists(string path);
    bool FileExists(string path);
}

public sealed class FileSystem : IFileSystem
{
    public bool DirectoryExists(string path)
        => Directory.Exists(path);

    public bool FileExists(string path)
        => File.Exists(path);
}