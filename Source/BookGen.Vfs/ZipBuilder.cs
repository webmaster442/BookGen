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

    public void Add(string entryName,
                    string entryValue,
                    Encoding encoding,
                    CompressionLevel compressionLevel = CompressionLevel.SmallestSize)
    {
        var entry = _archive.CreateEntry(entryName, compressionLevel);
        using var entryStream = entry.Open();
        using var writer = new StreamWriter(entryStream, encoding);
        writer.Write(entryValue);
    }

    public void Add(string entryName,
                    Stream entryValue,
                    CompressionLevel compressionLevel = CompressionLevel.SmallestSize)
    {
        var entry = _archive.CreateEntry(entryName, compressionLevel);
        using var entryStream = entry.Open();
        entryValue.CopyTo(entryStream);
    }

    public void Add(string entryName,
                    byte[] entryValue,
                    CompressionLevel compressionLevel = CompressionLevel.NoCompression)
    {
        var entry = _archive.CreateEntry(entryName, compressionLevel);
        using var entryStream = entry.Open();
        entryStream.Write(entryValue, 0, entryValue.Length);
    }

    public void AddXml<T>(string entryName,
                          T instance,
                          params (string prefix, string @namespace)[] namespaces)
    {

        //string? defaultNamespace = namespaces.FirstOrDefault(d => string.IsNullOrEmpty(d.prefix)).@namespace;

        XmlSerializer serializer = new(typeof(T)); // defaultNamespace);

        XmlSerializerNamespaces? xnames = null;
        if (namespaces?.Length > 0)
        {
            xnames = new XmlSerializerNamespaces();
            foreach ((string prefix, string namespac) in namespaces)
            {
                xnames.Add(prefix, namespac);
            }
        }

        var entry = _archive.CreateEntry(entryName, CompressionLevel.SmallestSize);
        using var entryStream = entry.Open();

        serializer.Serialize(entryStream, instance, xnames);
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, _archive);
        _archive.Dispose();
        _disposed = true;
    }
}