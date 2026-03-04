//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Markdown;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("md2terminal")]
internal sealed class Md2TerminalCommand : Command<Md2TerminalCommand.Arguments>
{
    private readonly ILogger _log;
    private readonly IWritableFileSystem _fileSystem;

    internal sealed class Arguments : ArgumentsBase
    {
        [Switch("i", "input")]
        public string[] InputFiles { get; set; }

        [Switch("o", "output")]
        public string OutputFile { get; set; }

        public Arguments()
        {
            InputFiles = [];
            OutputFile = string.Empty;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();

            if (string.IsNullOrEmpty(OutputFile))
                result.AddIssue("Output file must be specified");

            if (InputFiles.Length == 0)
                result.AddIssue("An Input file must be specified");

            foreach (var inputfile in InputFiles)
            {
                if (!context.FileSystem.FileExists(inputfile))
                    result.AddIssue($"Input file: {inputfile} doesn't exist");
            }

            return base.Validate(context);
        }
    }

    public Md2TerminalCommand(ILogger log, IWritableFileSystem fileSystem)
    {
        _log = log;
        _fileSystem = fileSystem;
    }

    private static void WriteToStdout(string rendered)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Spectre.Console.AnsiConsole.WriteLine(rendered);
    }

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        (string md, _) = _fileSystem.ReadInputFiles(arguments.InputFiles);

        using var settings = new MarkdownRenderSettings(null!)
        {
            DeleteFirstH1 = false,
            AutoEmbedSupportedLinks = false,
            CssClasses = new CssClasses(),
            HostUrl = string.Empty,
            PrismJsInterop = null,
            ImageRenderJsInterop = null!
        };

        using var markdonwConverter = new MarkdownConverter(settings);

        var rendered = markdonwConverter.RenderMarkdownToTerminal(md);

        if (arguments.OutputFile == "-")
            WriteToStdout(rendered);
        else
            _fileSystem.WriteAllText(arguments.OutputFile, rendered);

        return ExitCodes.Success;
    }
}
