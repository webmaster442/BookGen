//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

namespace BookGen.Vfs;

public sealed class ZipBuilder : IZipBuilder
{
    private readonly ZipArchive _archive;
    private bool _disposed;

    public ZipBuilder(string fileName)
    {
        _archive = new ZipArchive(
            stream: File.Create(fileName),
            mode: ZipArchiveMode.Create,
            leaveOpen: false,
            entryNameEncoding: Encoding.UTF8);
    }

    public async Task AddAsync(string entryName,
                               string entryValue,
                               Encoding encoding,
                               CompressionLevel compressionLevel = CompressionLevel.SmallestSize)
    {
        ZipArchiveEntry entry = _archive.CreateEntry(entryName, compressionLevel);
        await using Stream entryStream = entry.Open();
        await using var writer = new StreamWriter(entryStream, encoding);
        await writer.WriteAsync(entryValue);
    }

    public async Task AddAsync(string entryName,
                               Stream entryValue,
                               CompressionLevel compressionLevel = CompressionLevel.SmallestSize)
    {
        ZipArchiveEntry entry = _archive.CreateEntry(entryName, compressionLevel);
        await using Stream entryStream = entry.Open();
        await entryValue.CopyToAsync(entryStream);
    }

    public async Task AddAsync(string entryName,
                               byte[] entryValue,
                               CompressionLevel compressionLevel = CompressionLevel.NoCompression)
    {
        ZipArchiveEntry entry = _archive.CreateEntry(entryName, compressionLevel);
        await using Stream entryStream = entry.Open();
        await entryStream.WriteAsync(entryValue, 0, entryValue.Length);
    }

    public void AddXml<T>(string entryName,
                          T instance,
                          params (string prefix, string @namespace)[] namespaces)
    {
        XmlSerializer serializer = new(typeof(T));

        XmlSerializerNamespaces? xnames = null;
        if (namespaces?.Length > 0)
        {
            xnames = new XmlSerializerNamespaces();
            foreach ((string prefix, string namespac) in namespaces)
            {
                xnames.Add(prefix, namespac);
            }
        }

        ZipArchiveEntry entry = _archive.CreateEntry(entryName, CompressionLevel.SmallestSize);
        using Stream entryStream = entry.Open();

        serializer.Serialize(entryStream, instance, xnames);
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, _archive);
        _archive.Dispose();
        _disposed = true;
    }
}
