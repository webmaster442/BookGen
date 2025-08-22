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

    public byte[] GetBinaryAsset(string name)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_zip));
        lock (_lock)
        {
            ZipArchiveEntry? entry = _zip.GetEntry(name);
            if (entry == null)
            {
                throw new InvalidOperationException($"{name} was not found in assets");
            }

            byte[] data = new byte[entry.Length];
            using Stream dataStream = entry.Open();
            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
            int read = 0;
            int offset = 0;
            while ((read = dataStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                Array.Copy(buffer, 0, data, offset, read);
                offset += read;
            }
            ArrayPool<byte>.Shared.Return(buffer, true);

            return data;
        }
    }
}
