//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace BookGen.Settings;

public sealed partial class SettingsManager
{
    private readonly string _fileName;
    private readonly JsonSerializerOptions _options;

    public SettingsManager(string fileName)
    {
        _fileName = fileName;
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultBufferSize = 4096,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            Converters =
            {
                new JsonStringEnumConverter(),
            },
        };
    }

    [GeneratedRegex(@"^[a-zA-Z_0-9]+$")]
    private static partial Regex KeyValidator();

    public IEnumerable<(string key, DateTime utcLastModified)> Entries
    {
        get
        {
            if (!File.Exists(_fileName))
                yield break;

            using (var zipFile = File.OpenRead(_fileName))
            {
                using (var archive = new ZipArchive(zipFile, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        yield return (entry.Name, entry.LastWriteTime.UtcDateTime);
                    }
                }
            }
        }
    }

    public async Task<T?> DeserializeAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        if (!KeyValidator().IsMatch(key))
            throw new ArgumentException("key must only contain letters, numbers and the _ symbol");

        if (!File.Exists(_fileName))
        {
            return null;
        }

        using (var zipFile = File.OpenRead(_fileName))
        {
            using (var archive = new ZipArchive(zipFile, ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry(key);
                if (entry == null)
                {
                    return null;
                }

                using (var stream = entry.Open())
                {
                    return await JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken);
                }

            }
        }
    }

    public async Task SerializeAsync<T>(string key, T data, CancellationToken cancellationToken = default) where T : class
    {
        if (!KeyValidator().IsMatch(key))
            throw new ArgumentException("key must only contain letters, numbers and the _ symbol");

        using (var zipFile = File.Open(_fileName, FileMode.OpenOrCreate))
        {
            using (var archive = new ZipArchive(zipFile, ZipArchiveMode.Update))
            {
                var entry = archive.GetEntry(key);

                if (entry == null)
                    entry = archive.CreateEntry(key, CompressionLevel.Fastest);

                entry.LastWriteTime = DateTime.UtcNow;

                using (var stream = entry.Open())
                {
                    await JsonSerializer.SerializeAsync<T>(stream, data, _options, cancellationToken);
                }
            }
        }
    }
}
