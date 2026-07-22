//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;
using System.Text.Json;
using System.Text.Json.Schema;

using BookGen.Cli;
using BookGen.Cli.OpenCli;
using BookGen.Cli.OpenCli.Draft;
using BookGen.Lib;
using BookGen.Lib.Domain.IO;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Vfs;

namespace BookGen.Infrastructure;

internal sealed class DynamicDocumentGenerator : IDynamicDocumentGenerator
{
    private readonly ICommandRunnerProxy _commandRunnerProxy;

    public DynamicDocumentGenerator(ICommandRunnerProxy commandRunnerProxy)
    {
        _commandRunnerProxy = commandRunnerProxy;
    }

    public string GenerateCommandsDocument()
    {
        Document openCliDocs = _commandRunnerProxy.GetOpenCliDocs();
        StringBuilder commandsDoc = new(openCliDocs.Commands?.Count * 1024 ?? 1024);
        commandsDoc
            .AppendLine("# Commands")
            .AppendLine();

        foreach (Cli.OpenCli.Draft.Command command in openCliDocs.Commands?.OrderBy(x => x.Name) ?? Enumerable.Empty<Cli.OpenCli.Draft.Command>())
        {
            var cmd = MarkdownGenerator.GenerateMarkdown(command, 2);
            commandsDoc
                .Append(cmd);
        }

        return commandsDoc.ToString();
    }

    public string GenerateSchemasDocument()
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
