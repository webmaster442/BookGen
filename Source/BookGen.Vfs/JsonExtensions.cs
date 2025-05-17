using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using System.Text.Json.Serialization;

namespace BookGen.Vfs;

public static class JsonExtensions
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

    public static async Task WriteSchema<T>(this IWritableFileSystem fs, string path)
    {
        await using var stream = fs.CreateWriteStream(path);
        JsonNode schema = _options.GetJsonSchemaAsNode(typeof(T));
        using var writer = new StreamWriter(stream);
        await writer.WriteAsync(schema.ToJsonString());
    }
}
