using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli;

public interface ICommandHelpProvider
{
    void CommandsChanged(Document openCliDocument);
    string GetHelp(string commandName);
}
