//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;

namespace BookGen.Vfs;

public sealed class ZipAssetSoruce : IAssetSource, IDisposable
{
    private readonly ZipArchive _zip;
    private readonly ConcurrentDictionary<string, string> _cache;
    private readonly Lock _lock;
    private bool _disposed;

    public IReadOnlyList<string> AssetNames { get; }

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
        AssetNames = _zip.Entries.Select(e => e.FullName).ToArray();
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_zip));
        _zip.Dispose();
        _disposed = true;
    }

    public bool TryGetAsset(string name, [NotNullWhen(true)] out string? content)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_zip));

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

    public Stream GetBinaryAssetStream(string name)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_zip));
        lock (_lock)
        {
            ZipArchiveEntry? entry = _zip.GetEntry(name) ?? throw new InvalidOperationException($"{name} was not found in assets");
            return entry.Open();
        }
    }
}
