using System.Text.Json;
using System.Text.Json.Serialization;

using Bookgen.Lib.VFS;

namespace Bookgen.Lib.Internals;

internal static class JsonExtensions
{
    private readonly static JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public static async Task<T?> DeserializeAsync<T>(this IFolder folder, string path)
    {
        await using var stream = folder.OpenStream(path);
        T? result = await JsonSerializer.DeserializeAsync<T>(stream, _options);
        return result;
    }

    public static async Task SerializeAsync<T>(this IFolder folder, string path, T value)
    {
        await using var stream = folder.CreateStream(path);
        await JsonSerializer.SerializeAsync<T>(stream, value, _options);
    }
}
