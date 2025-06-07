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
        TransformSchemaNode = (context, schema) =>
        {
            // Determine if a type or property and extract the relevant attribute provider.
            ICustomAttributeProvider? attributeProvider = context.PropertyInfo is not null
                ? context.PropertyInfo.AttributeProvider
                : context.TypeInfo.Type;

            // Look up any description attributes.
            DescriptionAttribute? descriptionAttr = attributeProvider?
                .GetCustomAttributes(inherit: true)
                .Select(attr => attr as DescriptionAttribute)
                .FirstOrDefault(attr => attr is not null);

            // Apply description attribute to the generated schema.
            if (descriptionAttr != null)
            {
                if (schema is not JsonObject jObj)
                {
                    // Handle the case where the schema is a Boolean.
                    JsonValueKind valueKind = schema.GetValueKind();
                    Debug.Assert(valueKind is JsonValueKind.True or JsonValueKind.False);
                    schema = jObj = new JsonObject();
                    if (valueKind is JsonValueKind.False)
                    {
                        jObj.Add("not", true);
                    }
                }

                jObj.Insert(0, "description", descriptionAttr.Description);
            }

            return schema;
        }
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

    private static async Task WriteSchema<T>(this IWritableFileSystem fs, string path)
    {
        var schemaString = _options
            .GetJsonSchemaAsNode(typeof(T), _exporterOptions)
            .ToString();

        await fs.WriteAllTextAsync(path, schemaString);
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
