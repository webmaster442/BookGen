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

namespace BookGen.Commands.Docs;

[CommandName("document commands")]
[Description("Displays commands reference on the terminal. Output can be redirected to a file.")]
internal sealed class CommandsCommand : DocumentCommandBase
{
    private readonly ICommandRunnerProxy _commandRunnerProxy;

    public CommandsCommand(ICommandRunnerProxy commandRunnerProxy)
    {
        _commandRunnerProxy = commandRunnerProxy;
    }

    protected override string GetDocumentContent()
    {
        Document openCliDocs = _commandRunnerProxy.GetOpenCliDocs();
        StringBuilder commandsDoc = new(openCliDocs.Commands?.Count * 1024 ?? 1024);
        commandsDoc
            .AppendLine("# Commands")
            .AppendLine();

        foreach (Cli.OpenCli.Draft.Command command in openCliDocs?.Commands?.OrderBy(x => x.Name) ?? Enumerable.Empty<Cli.OpenCli.Draft.Command>())
        {
            var cmd = MarkdownGenerator.GenerateMarkdown(command, 2);
            commandsDoc
                .Append(cmd);
        }

        return commandsDoc.ToString();
    }
}
