//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Vfs;

public sealed class FileSystemFactory : IFileSystemFactory
{
    public IReadOnlyFileSystem CreateMultiReadScopeFileSystem(params IEnumerable<string> scopes)
        => new MultiReadScopeFileSystem(scopes);

    public IReadOnlyFileSystem CreateReadOnlyFileSystem(string scope = "")
    {
        return new ReadOnlyFileSystem
        {
            Scope = scope
        };
    }

    public IWritableFileSystem CreateWritableFileSystem(string scope = "")
    {
        return new FileSystem
        {
            Scope = scope
        };
    }
}
