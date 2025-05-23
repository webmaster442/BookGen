using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("help")]
internal class HelpCommand : Command
{
    private readonly IHelpProvider _helpProvider;
    private readonly HashSet<string> _commandNames;

    public HelpCommand(IHelpProvider helpProvider, ICommandRunnerProxy runnerProxy)
    {
        _helpProvider = helpProvider;
        _commandNames = [.. runnerProxy.CommandNames];
    }

    public override int Execute(string[] context)
    {
        if (context.Length == 0) 
        {
            HelpRenderer.RenderHelp(_helpProvider.GetCommandHelp("help"));
            return ExitCodes.Succes;
        }

        string command = context[0].ToLower();
        if (!_commandNames.Contains(command))
        {
            AnsiConsole.WriteLine("Unknown Command: {0}", command);
            return ExitCodes.GeneralError;
        }

        HelpRenderer.RenderHelp(_helpProvider.GetCommandHelp(command));
        return ExitCodes.Succes;

    }
}
