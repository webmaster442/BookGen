using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Schema;

using BookGen.Cli.Annotations;
using BookGen.Lib;
using BookGen.Lib.Domain.IO;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Vfs;

namespace BookGen.Commands.Docs;

[CommandName("document schemas")]
[Description("Outputs the JSON schemas used by Bookgen on the terminal. Output can be redirected to a file.")]
internal class SchemasCommand : DocumentCommandBase
{
    protected override string GetDocumentContent()
    {
        JsonSerializerOptions options = JsonOptions.SerializerOptions;
        JsonSchemaExporterOptions exporterOptions = JsonOptions.ExporterOptions;

        MarkdownBuilder markdownBuilder = new();

        markdownBuilder.Heading(1, "Bookgen Schemas")
            .Paragraph("This document contains the schemas used by Bookgen.")
            .Heading(2, "Bookgen.json")
            .CodeBlock(options.GetJsonSchemaAsNode(typeof(Config), exporterOptions).ToString(), "json")
            .Heading(2, "Table of contents file")
            .CodeBlock(options.GetJsonSchemaAsNode(typeof(TableOfContents), exporterOptions).ToString(), "json")
            .Heading(3, "Page frontmatter")
            .Paragraph("Each page in the table of contents must have a YAML front matter.")
            .CodeBlock(options.GetJsonSchemaAsNode(typeof(FrontMatter), exporterOptions).ToString(), "json");

        return markdownBuilder.ToString();
    }
}
