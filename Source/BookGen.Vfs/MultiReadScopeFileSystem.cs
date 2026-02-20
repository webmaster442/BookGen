//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Vfs;

public sealed class MultiReadScopeFileSystem : IReadOnlyFileSystem
{
    private readonly HashSet<string> _scopes;

    private string Resolve(string path)
    {
        foreach (var scope in _scopes)
        {
            var fullPath = Path.GetFullPath(Path.Combine(scope, path));
            if (File.Exists(fullPath)) 
                return fullPath;
        }
        throw new InvalidOperationException($"{path} can't be found");
    }

    public MultiReadScopeFileSystem(params IEnumerable<string> scopes)
    {
        _scopes = new HashSet<string>(scopes);
    }

    public string Scope 
    {
        get => string.Join(Environment.NewLine, _scopes);
        set => throw new NotSupportedException();
    }

    public bool DirectoryExists(string path)
    {
        foreach (var scope in _scopes)
        {
            var fullPath = Path.GetFullPath(Path.Combine(scope, path));

            if (Directory.Exists(fullPath))
                return true;
        }
        return false;
    }

    public bool FileExists(string path)
    {
        foreach (var scope in _scopes)
        {
            var fullPath = Path.GetFullPath(Path.Combine(scope, path));

            if (File.Exists(fullPath))
                return true;
        }
        return false;
    }

    public IEnumerable<string> GetDirectories(string path, bool recursive)
    {
        foreach (var scope in _scopes)
        {
            string fullPath = Path.GetFullPath(Path.Combine(scope, path));
            IEnumerable<string> directories = Directory.EnumerateDirectories(fullPath, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var directory in directories)
            {
                yield return directory;
            }
        }
    }

    public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
    {
        foreach (var scope in _scopes)
        {
            string fullPath = Path.GetFullPath(Path.Combine(scope, path));
            IEnumerable<string> files = Directory.EnumerateFiles(fullPath, filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                yield return file;
            }
        }
    }

    public long GetFileSize(string path)
    {
        var actualPath = Resolve(path);
        FileInfo fileInfo = new FileInfo(actualPath);
        return fileInfo.Length;
    }

    public DateTime GetLastModifiedUtc(string path)
    {
        var actualPath = Resolve(path);
        return Directory.Exists(actualPath)
            ? Directory.GetLastWriteTimeUtc(actualPath)
            : File.GetLastWriteTimeUtc(actualPath);
    }

    public Stream OpenReadStream(string path)
    {
        var actualPath = Resolve(path);
        return File.OpenRead(actualPath);
    }

    public TextReader OpenTextReader(string path)
    {
        var actualPath = Resolve(path);
        return File.OpenText(actualPath);
    }

    public string ReadAllText(string path)
    {
        var actualPath = Resolve(path);
        return File.ReadAllText(actualPath);
    }

    public async Task<string> ReadAllTextAsync(string path)
    {
        var actualPath = Resolve(path);
        return await File.ReadAllTextAsync(actualPath);
    }
}
