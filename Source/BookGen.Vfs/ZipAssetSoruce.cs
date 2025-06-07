using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;

namespace BookGen.Vfs;

public sealed class ZipAssetSoruce : IAssetSource, IDisposable
{
    private readonly ZipArchive _zip;
    private bool _disposed;
    private readonly ConcurrentDictionary<string, string> _cache;
    private readonly Lock _lock;

    public static ZipAssetSoruce DefaultAssets()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "assets.zip");
        return new ZipAssetSoruce(path);
    }

    public ZipAssetSoruce(string fileName)
    {
        _cache = new ConcurrentDictionary<string, string>();
        _zip = ZipFile.OpenRead(fileName);
        _lock = new Lock();
    }

    public void Dispose()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(_zip));

        _zip.Dispose();
        _disposed = true;
    }

    public bool TryGetAsset(string name, [NotNullWhen(true)] out string? content)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(_zip));

        if (_cache.TryGetValue(name, out content))
        {
            return true;
        }

        lock (_lock)
        {

            ZipArchiveEntry? entry = _zip.GetEntry(name);

            if (entry == null)
            {
                content = null;
                return false;
            }

            using Stream dataStream = entry.Open();
            using StreamReader reader = new StreamReader(dataStream);

            content = reader.ReadToEnd();
            _cache.TryAdd(name, content);
        }

        return true;
    }
}
