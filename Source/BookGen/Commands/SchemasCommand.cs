using System.Text.Json;
using System.Text.Json.Schema;
using System.Text.Json.Serialization;

using Bookgen.Lib;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

namespace BookGen.Commands;

[CommandName("schemas")]
internal class SchemasCommand : Command<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly JsonSerializerOptions _options;

    public SchemasCommand(IWritableFileSystem writableFileSystem)
    {
        _writableFileSystem = writableFileSystem;
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
    }

    public override int Execute(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        MarkdownBuilder markdownBuilder = new();

        markdownBuilder.Heading(1, "Bookgen Schemas")
            .Paragraph("This document contains the schemas used by Bookgen.")
            .Heading(2, "Bookgen.json")
            .CodeBlock(_options.GetJsonSchemaAsNode(typeof(Config)).ToJsonString(), "json")
            .Heading(2, "Table of contents file")
            .CodeBlock(_options.GetJsonSchemaAsNode(typeof(TableOfContents)).ToJsonString(), "json")
            .Heading(3, "Page frontmatter")
            .Paragraph("Each page in the table of contents must have a YAML front matter.")
            .CodeBlock(_options.GetJsonSchemaAsNode(typeof(FrontMatter)).ToJsonString(), "json");

        _writableFileSystem.Scope = arguments.Directory;
        _writableFileSystem.WriteAllText("schemas.md", markdownBuilder.ToString());

        return ExitCodes.Succes;
    }
}