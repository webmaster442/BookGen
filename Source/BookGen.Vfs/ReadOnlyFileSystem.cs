using System.Diagnostics;

namespace BookGen.Vfs;

[DebuggerDisplay("{Scope}")]
public class ReadOnlyFileSystem : IReadOnlyFileSystem
{
    protected string GetAndValidateFullNameInScope(string path)
    {
        string returnValue = string.IsNullOrEmpty(Scope)
            ? Path.GetFullPath(path)
            : Path.GetFullPath(path, Scope);

        if (!returnValue.StartsWith(Scope))
            throw new InvalidOperationException($"{returnValue} points out of defined scope: {Scope}");

        return returnValue;
    }

    public ReadOnlyFileSystem(string scope = "")
    {
        Scope = scope;
    }

    public string Scope
    {
        get => field;
        set
        {
            field = string.IsNullOrEmpty(value)
                ? value
                : Path.GetFullPath(value);
        }
    }

    public bool DirectoryExists(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return Directory.Exists(actualPath);
    }

    public bool FileExists(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.Exists(actualPath);
    }

    public IEnumerable<string> GetDirectories(string path, bool recursive)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return Directory.EnumerateDirectories(actualPath, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    }

    public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return Directory.EnumerateFiles(actualPath, filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    }

    public long GetFileSize(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        FileInfo fileInfo = new FileInfo(actualPath);
        return fileInfo.Length;
    }

    public DateTime GetLastModifiedUtc(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return Directory.Exists(actualPath)
            ? Directory.GetLastWriteTimeUtc(actualPath)
            : File.GetLastWriteTimeUtc(actualPath);
    }

    public Stream OpenReadStream(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.OpenRead(actualPath);
    }

    public TextReader OpenTextReader(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.OpenText(actualPath);
    }

    public string ReadAllText(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.ReadAllText(actualPath);
    }

    public async Task<string> ReadAllTextAsync(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return await File.ReadAllTextAsync(actualPath);
    }
}
