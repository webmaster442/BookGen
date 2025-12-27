//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Templates;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

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

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        (string md, DateTime lastmodified) = _fileSystem.ReadInputFiles(arguments.InputFiles);
    }
}
