using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;

namespace BookGen.Vfs;

internal sealed class ZipAssetSoruce : IAssetSource, IDisposable
{
    private readonly ZipArchive _zip;
    private bool _disposed;

    public ZipAssetSoruce(string fileName)
    {
        _zip = ZipFile.OpenRead(fileName);
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

        ZipArchiveEntry? entry = _zip.GetEntry(name);

        if (entry == null)
        {
            content = null;
            return false;
        }

        using Stream dataStream = entry.Open();
        using StreamReader reader = new StreamReader(dataStream);

        content = reader.ReadToEnd();
        return true;
    }
}
