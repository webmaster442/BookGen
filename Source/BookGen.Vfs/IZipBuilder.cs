using System.IO.Compression;
using System.Text;

namespace BookGen.Vfs;

public interface IZipBuilder : IDisposable
{
    void Add(string entryName,
             string entryValue,
             Encoding encoding,
             CompressionLevel compressionLevel = CompressionLevel.SmallestSize);
     void Add(string entryName,
              Stream entryValue,
              CompressionLevel compressionLevel = CompressionLevel.SmallestSize);

    void Add(string entryName,
             byte[] entryValue,
             CompressionLevel compressionLevel = CompressionLevel.NoCompression);

    void AddXml<T>(string entryName,
                   T instance,
                   params (string prefix, string @namespace)[] namespaces);
}
