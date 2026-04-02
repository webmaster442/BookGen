//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Vfs;

/// <summary>
/// Implements both IReadOnlyFileSystem and IWritableFileSystem, but all write operations do nothing.
/// </summary>
[DebuggerDisplay("{Scope}")]
public sealed class ReadOnlyWritableFileSystem : ReadOnlyFileSystem, IReadOnlyFileSystem, IWritableFileSystem
{
    public Task CopyToAsync(string path, string destination)
        => Task.CompletedTask;

    public void CreateDirectoryIfNotExist(string path)
    {
        // do nothing
    }

    public TextWriter CreateTextWriter(string path)
    {
        return new StreamWriter(new NullStream());
    }

    public Stream CreateWriteStream(string path)
        => new NullStream();

    public void Delete(string path)
    {
        // do nothing
    }

    public void MoveFile(string path, string destination)
    {
        // do nothing
    }

    public void WriteAllText(string path, string content)
    {
        // do nothing
    }

    public Task WriteAllTextAsync(string path, string content)
        => Task.CompletedTask;
}
