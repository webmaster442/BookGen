//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.V1;

namespace BookGen.PluginInfrastructure.V1;

internal sealed class FileSystem : IFileSystem
{
    private readonly Vfs.IWritableFileSystem _fileSystem;

    public FileSystem(Vfs.IWritableFileSystem writableFileSystem)
    {
        _fileSystem = writableFileSystem;
    }

    public bool FileExists(string relativePath)
        => _fileSystem.FileExists(relativePath);

    public Task WriteTextFile(string relativePath, string content)
        => _fileSystem.WriteAllTextAsync(relativePath, content);
}
