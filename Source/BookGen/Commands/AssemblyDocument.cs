using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

using XmlDocMarkdown.Core;

namespace BookGen.Commands;

[CommandName("assembly-document")]
internal class AssemblyDocument : Command<AssemblyDocument.Arguments>
{
    private readonly ILogger _logger;

    public AssemblyDocument(ILogger logger)
    {
        _logger = logger;
    }

    public class Arguments : InputOutputArguments
    {
        [Switch("d", "dry")]
        public bool DryRun { get; set; }

        [Switch("n", "namespace-pages")]
        public bool NamespacePages { get; set; }
    }

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        var result = XmlDocMarkdownGenerator.Generate(arguments.InputFile, arguments.OutputFile, new XmlDocMarkdownSettings
        {
            IsDryRun = arguments.DryRun,
            IncludeObsolete = true,
            NamespacePages = arguments.NamespacePages,
            VisibilityLevel = XmlDocVisibilityLevel.Public,
            ShouldClean = true,
            SkipUnbrowsable = true,
        });

        foreach (string message in result.Messages)
        {
            _logger.LogInformation(message);
        }

        return ExitCodes.Succes;

    }
}
