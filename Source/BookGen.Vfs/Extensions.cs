using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using BookGen.Vfs.Internals;

namespace BookGen.Vfs;

public static class Extensions
{
    private readonly static JsonSerializerOptions _options = new(JsonSerializerOptions.Default)
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            new CultureInfoJsonConverter()
        }
    };

    private static readonly JsonSchemaExporterOptions _exporterOptions = new()
    {
        TransformSchemaNode = JsonSchemaTransformer.TransformSchemaNode
    };

    public static async Task<T?> DeserializeAsync<T>(this IReadOnlyFileSystem fs, string path)
    {
        await using var stream = fs.OpenReadStream(path);
        T? result = await JsonSerializer.DeserializeAsync<T>(stream, _options);
        return result;
    }

    public static async Task SerializeAsync<T>(this IWritableFileSystem fs, string path, T value, bool writeSchema)
    {
        await using var stream = fs.CreateWriteStream(path);
        await JsonSerializer.SerializeAsync(stream, value, _options);
        if (writeSchema)
        {
            var newName = Path.ChangeExtension(path, ".schema.json");
            await fs.WriteSchema<T>(newName);
        }
    }

    public static async Task WriteSchema<T>(this IWritableFileSystem fs, string path)
    {
        var node = _options.GetJsonSchemaAsNode(typeof(T), _exporterOptions);
        await fs.WriteAllTextAsync(path, node.ToString());
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
