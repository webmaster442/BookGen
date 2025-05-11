using Bookgen.Lib.VFS;

namespace Bookgen.Tests;

internal class EmbeddedTestFolder : IReadOnlyFolder
{
    private readonly Dictionary<string, string> _table;

    public EmbeddedTestFolder()
    {
        _table = typeof(EmbeddedTestFolder).Assembly
            .GetManifestResourceNames()
            .ToDictionary(x => string.Join('.', x.Split('.').TakeLast(2)), x => x);
    }

    public string FullPath => "/";

    public bool Exists(string path)
        => _table.ContainsKey(path);

    public IEnumerable<string> GetDirectories(bool recursive)
        => Enumerable.Empty<string>();

    public IEnumerable<string> GetFiles(bool recursive)
        => _table.Keys;

    public DateTime GetLastModifiedUtc(string path)
        => DateTime.UtcNow;

    public Stream OpenStream(string path)
    {
        var actualPath = _table[path];
        return typeof(EmbeddedTestFolder).Assembly.GetManifestResourceStream(actualPath)
            ?? throw new InvalidOperationException();
    }

    public TextReader OpenText(string path)
    {
        using var stream = OpenStream(path);
        return new StreamReader(stream);
    }

    public string ReadText(string path)
    {
        using var stream = OpenStream(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
