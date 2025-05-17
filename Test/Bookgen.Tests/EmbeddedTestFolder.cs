
using BookGen.Vfs;

namespace Bookgen.Tests;

internal class EmbeddedTestFileSystem : IReadOnlyFileSystem
{
    private readonly Dictionary<string, string> _table;

    public EmbeddedTestFileSystem()
    {
        Scope = string.Empty;
        _table = typeof(EmbeddedTestFileSystem).Assembly
            .GetManifestResourceNames()
            .ToDictionary(x => string.Join('.', x.Split('.').TakeLast(2)), x => x);
    }

    public string Scope { get; set; }

    public bool DirectoryExists(string path)
    {
        throw new NotImplementedException();
    }

    public bool FileExists(string path)
        => _table.ContainsKey(path);

    public IEnumerable<string> GetDirectories(string path, bool recursive)
        => Enumerable.Empty<string>();

    public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        => _table.Keys;

    public DateTime GetLastModifiedUtc(string path)
        => DateTime.UtcNow;

    public Stream OpenReadStream(string path)
    {
        var actualPath = _table[path];
        return typeof(EmbeddedTestFileSystem).Assembly.GetManifestResourceStream(actualPath)
            ?? throw new InvalidOperationException();
    }

    public TextReader OpenTextReader(string path)
    {
        using var stream = OpenReadStream(path);
        return new StreamReader(stream);
    }

    public string ReadAllText(string path)
    {
        using var stream = OpenReadStream(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public async Task<string> ReadAllTextAsync(string path)
    {
        using var stream = OpenReadStream(path);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
