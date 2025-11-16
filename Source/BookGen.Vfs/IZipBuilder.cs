//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO.Compression;
using System.Text;

namespace BookGen.Vfs;

public interface IZipBuilder : IDisposable
{
    Task AddAsync(string entryName,
                  string entryValue,
                  Encoding encoding,
                  CompressionLevel compressionLevel = CompressionLevel.SmallestSize);
    Task AddAsync(string entryName,
                  Stream entryValue,
                  CompressionLevel compressionLevel = CompressionLevel.SmallestSize);

    Task AddAsync(string entryName,
                  byte[] entryValue,
                  CompressionLevel compressionLevel = CompressionLevel.NoCompression);

    void AddXml<T>(string entryName,
                   T instance,
                   params (string prefix, string @namespace)[] namespaces);
}
