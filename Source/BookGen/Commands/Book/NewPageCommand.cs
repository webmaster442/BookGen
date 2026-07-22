//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Lib;
using BookGen.Lib.Domain.IO;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using YamlDotNet.Serialization;

namespace BookGen.Commands.Book;

[CommandName("book newpage")]
[Description("Creates a new markdown page.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class NewPageCommand : Command<NewPageCommand.Arguments>
{
    internal sealed class Arguments : BookGenArgumentBase
    {
        [Switch("-n", "name", Required = true)]
        [Description("File name. Specifies new file name")]
        public string Name { get; set; } = string.Empty;

        public override ValidationResult Validate(IValidationContext context)
        {
            if (string.IsNullOrEmpty(Name))
                return ValidationResult.Error("No file name specified");

            return ValidationResult.Ok();
        }

        public override void ModifyAfterValidation()
        {
            if (!string.Equals(Path.GetExtension(Name), ".md", StringComparison.OrdinalIgnoreCase))
                Name = Path.ChangeExtension(Name, ".md");
        }
    }

    private readonly ILogger _logger;
    private readonly IWritableFileSystem _fileSystem;

    public NewPageCommand(ILogger logger, IWritableFileSystem fileSystem)
    {
        _logger = logger;
        _fileSystem = fileSystem;
    }

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        FrontMatter frontMatter = new()
        {
            Title = "New page",
            Tags = "",
        };

        ISerializer serializer = YamlSerializerFactory.CreateSerializer();

        var yaml = serializer.Serialize(frontMatter);

        string content =
            $"""
            ---
            {frontMatter}
            ---
            # New page

            """;

        var fileName = Path.Combine(arguments.Directory, arguments.Name);

        _fileSystem.WriteAllText(fileName, content);
        _logger.LogInformation("Created new page at {fileName}", fileName);

        return ExitCodes.Success;

    }
}
