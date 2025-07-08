using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace BookGen.Vfs;

public static class Extensions
{
    public static async Task<T?> DeserializeAsync<T>(this IReadOnlyFileSystem fs, string path)
    {
        await using var stream = fs.OpenReadStream(path);
        T? result = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions.SerializerOptions);
        return result;
    }

    public static async Task SerializeAsync<T>(this IWritableFileSystem fs, string path, T value, bool writeSchema)
    {
        await using var stream = fs.CreateWriteStream(path);
        await JsonSerializer.SerializeAsync(stream, value, JsonOptions.SerializerOptions);
        if (writeSchema)
        {
            var newName = Path.ChangeExtension(path, ".schema.json");
            await fs.WriteSchema<T>(newName);
        }
    }

    public static async Task WriteJsonAsync(this IWritableFileSystem fs, string path, JsonObject json)
    {
        await fs.WriteAllTextAsync(path, json.ToJsonString(JsonOptions.SerializerOptions));
    }

    public static async Task<JsonObject> ReadJsonAsync(this IReadOnlyFileSystem fs, string path)
    {
        string content = await fs.ReadAllTextAsync(path);
        var parsed = JsonObject.Parse(content);
        if (parsed is not JsonObject jsonObject)
        {
            throw new InvalidOperationException($"Failed to parse JSON from {path}");
        }
        return jsonObject;
    }

    public static async Task WriteSchema<T>(this IWritableFileSystem fs, string path)
    {
        var node = JsonOptions.SerializerOptions.GetJsonSchemaAsNode(typeof(T), JsonOptions.ExporterOptions);
        await fs.WriteAllTextAsync(path, node.ToJsonString(JsonOptions.SerializerOptions));
    }

    public static string GetFileNameInTargetFolder(this IReadOnlyFileSystem sourceFolder, IReadOnlyFileSystem targetFolder, string file, string newExtension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceFolder.Scope);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetFolder.Scope);

        var fullPath = Path.GetFullPath(file, sourceFolder.Scope);

        var relativePart = Path.GetRelativePath(sourceFolder.Scope, fullPath);

        return Path.ChangeExtension(Path.GetFullPath(relativePart, targetFolder.Scope), newExtension);
    }
}
