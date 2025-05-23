using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using Bookgen.Lib;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("schemas")]
internal class SchemasCommand : Command<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly ILogger _logger;
    private readonly JsonSerializerOptions _options;
    private readonly JsonSchemaExporterOptions _exporterOptions;

    public SchemasCommand(IWritableFileSystem writableFileSystem, ILogger logger)
    {
        _writableFileSystem = writableFileSystem;
        _logger = logger;
        _options = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter()
            },
        };
        _exporterOptions = new()
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
    }

    public override int Execute(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        MarkdownBuilder markdownBuilder = new();

        markdownBuilder.Heading(1, "Bookgen Schemas")
            .Paragraph("This document contains the schemas used by Bookgen.")
            .Heading(2, "Bookgen.json")
            .CodeBlock(_options.GetJsonSchemaAsNode(typeof(Config), _exporterOptions).ToString(), "json")
            .Heading(2, "Table of contents file")
            .CodeBlock(_options.GetJsonSchemaAsNode(typeof(TableOfContents), _exporterOptions).ToString(), "json")
            .Heading(3, "Page frontmatter")
            .Paragraph("Each page in the table of contents must have a YAML front matter.")
            .CodeBlock(_options.GetJsonSchemaAsNode(typeof(FrontMatter), _exporterOptions).ToString(), "json");

        _logger.LogInformation("Writing schemas.md...");
        _writableFileSystem.Scope = arguments.Directory;
        _writableFileSystem.WriteAllText("schemas.md", markdownBuilder.ToString());

        return ExitCodes.Succes;
    }
}