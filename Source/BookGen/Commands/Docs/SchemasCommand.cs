//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Schema;

using BookGen.Lib;
using BookGen.Lib.Domain.IO;
using BookGen.Lib.Domain.IO.Configuration;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Docs;

[CommandName("schemas")]
[Description("Creates a `schemas.md` documentation file, describing the various config schemas used by bookgen.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class SchemasCommand : Command<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly ILogger _logger;

    public SchemasCommand(IWritableFileSystem writableFileSystem, ILogger logger)
    {
        _writableFileSystem = writableFileSystem;
        _logger = logger;
    }

    public override int Execute(BookGenArgumentBase arguments, IReadOnlyList<string> context)
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

        _logger.LogInformation("Writing schemas.md...");
        _writableFileSystem.Scope = arguments.Directory;
        _writableFileSystem.WriteAllText("schemas.md", markdownBuilder.ToString());

        return ExitCodes.Success;
    }
}
