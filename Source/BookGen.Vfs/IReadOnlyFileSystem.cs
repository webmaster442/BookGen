//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BookGen.Vfs;

public interface IReadOnlyFileSystem
{
    string Scope { get; set; }
    TextReader OpenTextReader(string path);
    Stream OpenReadStream(string path);
    DateTime GetLastModifiedUtc(string path);
    long GetFileSize(string path);
    string ReadAllText(string path);
    Task<string> ReadAllTextAsync(string path);
    bool FileExists(string path);
    bool DirectoryExists(string path);
    IEnumerable<string> GetFiles(string path, string filter, bool recursive);
    IEnumerable<string> GetDirectories(string path, bool recursive);
    IFileSystemObserver CreateObserver(ILogger logger, string filter = "*.*");
}
