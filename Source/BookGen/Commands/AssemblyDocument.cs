//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

using XmlDocMarkdown.Core;

namespace BookGen.Commands;

[CommandName("assembly-document")]
[Description("Generates a markdown file(s) from a given .NET assembly and it's XML documentation file.")]
[ExitCode(ExitCodes.Success, "The markdown file(s) was generated successfully.")]
internal sealed class AssemblyDocument : Command<AssemblyDocument.Arguments>
{
    private readonly ILogger _logger;

    public AssemblyDocument(ILogger logger)
    {
        _logger = logger;
    }

    public class Arguments : InputOutputArguments
    {
        [Switch("d", "dry", false)]
        [Description("Optional argument. If specified, the command will not generate any files, but will only log the actions that would be taken.")]
        public bool DryRun { get; set; }

        [Switch("n", "namespace-pages", false)]
        [Description("Optional argument. If specified, the command will create a separate markdown file for each namespace in the assembly.")]
        public bool NamespacePages { get; set; }
    }

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        XmlDocMarkdownResult result = XmlDocMarkdownGenerator.Generate(arguments.InputFile, arguments.OutputFile, new XmlDocMarkdownSettings
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

        return ExitCodes.Success;

    }
}
