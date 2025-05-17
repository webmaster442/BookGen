using Bookgen.Lib;
using Bookgen.Lib.Domain.IO;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("newpage")]
internal class NewPageCommand : Command<NewPageCommand.Arguments>
{
    internal class Arguments : BookGenArgumentBase
    {
        [Switch("-n", "name")]
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
                Path.ChangeExtension(Name, ".md");
        }
    }

    private readonly ILogger _logger;
    private readonly IWritableFileSystem _fileSystem;
    private readonly ProgramInfo _programInfo;

    public NewPageCommand(ILogger logger, IWritableFileSystem fileSystem, ProgramInfo programInfo)
    {
        _logger = logger;
        _fileSystem = fileSystem;
        _programInfo = programInfo;
    }

    public override int Execute(Arguments arguments, string[] context)
    {
        _programInfo.EableVerboseLogging(arguments.Verbose);
        FrontMatter frontMatter = new()
        {
            Title = "New page",
            Tags = "",
        };

        var serializer = YamlSerializerFactory.CreateSerializer();

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

        return ExitCodes.Succes;
        
    }
}
