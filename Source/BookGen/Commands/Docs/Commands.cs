//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Cli.OpenCli;
using BookGen.Cli.OpenCli.Draft;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Docs;

[CommandName("commands")]
[Description("Creates a `commands.md` documentation file, describing the various commands available in bookgen.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class Commands : Command<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly ILogger _logger;
    private readonly ICommandRunnerProxy _commandRunnerProxy;

    public Commands(IWritableFileSystem writableFileSystem, ILogger logger, ICommandRunnerProxy commandRunnerProxy)
    {
        _writableFileSystem = writableFileSystem;
        _logger = logger;
        _commandRunnerProxy = commandRunnerProxy;
    }

    public override int Execute(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        StringBuilder commandsDoc = new(4096);
        Document openCliDocs = _commandRunnerProxy.GetOpenCliDocs();
        commandsDoc
            .Append("# Commands")
            .AppendLine();

        foreach (var command in openCliDocs?.Commands ?? new List<Cli.OpenCli.Draft.Command>())
        {
            var cmd = MarkdownGenerator.GenerateMarkdown(command, 2);
            commandsDoc
                .Append(cmd)
                .AppendLine();
        }


        _logger.LogInformation("Writing commands.md...");
        _writableFileSystem.Scope = arguments.Directory;
        _writableFileSystem.WriteAllText("commands.md", commandsDoc.ToString());

        return ExitCodes.Success;
    }
}
