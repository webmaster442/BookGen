using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using BookGen.Vfs.Internals;

namespace BookGen.Vfs;

public static class Extensions
{
    private readonly static JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            new CultureInfoConverter()
        }
    };

    public static async Task<T?> DeserializeAsync<T>(this IReadOnlyFileSystem fs, string path)
    {
        await using var stream = fs.OpenReadStream(path);
        T? result = await JsonSerializer.DeserializeAsync<T>(stream, _options);
        return result;
    }

    public static async Task SerializeAsync<T>(this IWritableFileSystem fs, string path, T value)
    {
        await using var stream = fs.CreateWriteStream(path);
        await JsonSerializer.SerializeAsync(stream, value, _options);
    }

    public static string GetFileNameInTargetFolder(this IReadOnlyFileSystem sourceFolder, IReadOnlyFileSystem targetFolder, string file)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceFolder.Scope);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetFolder.Scope);

        var fullPath = Path.GetFullPath(file, sourceFolder.Scope);
        var relativePart = fullPath.Replace(sourceFolder.Scope, "");
        return Path.GetFullPath(relativePart, targetFolder.Scope);
    }
}
