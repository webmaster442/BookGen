//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.OpenCli;
using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli;

public class CommandHelpProvider : ICommandHelpProvider
{
    private Document _document;

    public CommandHelpProvider()
    {
        _document = new Document
        {
            Info = new CliInfo
            {
                Title = string.Empty,
                Version = string.Empty,
            },
            Command = new OpenCli.Draft.Command
            {
                Name = string.Empty,
            },
            Opencli = "0.1"
        };
    }

    public void CommandsChanged(Document openCliDocument)
    {
        _document = openCliDocument;
    }

    public virtual string GetHelp(string commandName)
    {
        OpenCli.Draft.Command? command = _document.Commands?.FirstOrDefault(c => c.Name == commandName);
        if (command == null)
            return $"** No Help found for: {commandName} **";

        return MarkdownGenerator.GenerateMarkdown(command, 1);
    }
}

