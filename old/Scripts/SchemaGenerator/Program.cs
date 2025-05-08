using System.Globalization;

using BookGen.Domain.Configuration;

using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

static async Task WriteSchemaToFile<T>(string output)
{
    var schema = JsonSchema.FromType<T>(new NJsonSchema.Generation.SystemTextJsonSchemaGeneratorSettings
    {
        TypeMappers = new ITypeMapper[]
        {
            new PrimitiveTypeMapper(typeof(CultureInfo), s =>
            {
                s.Type = JsonObjectType.String;
            })
        },
    });
    await File.WriteAllTextAsync(output, schema.ToJson());
}

Console.WriteLine("writing config.schema.json...");
await WriteSchemaToFile<Config>("config.schema.json");

Console.WriteLine("tags.schema.json...");
await WriteSchemaToFile<Dictionary<string, string[]>>("tags.schema.json");

Console.WriteLine("Done");
