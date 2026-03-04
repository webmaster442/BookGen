//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("help")]
internal sealed class HelpCommand : Command
{
    private readonly IHelpProvider _helpProvider;
    private readonly HashSet<string> _commandNames;
    private readonly HelpRenderer _renderer = new();

    public HelpCommand(IHelpProvider helpProvider, ICommandRunnerProxy runnerProxy)
    {
        _helpProvider = helpProvider;
        _commandNames = [.. runnerProxy.CommandNames];
    }

    public override int Execute(IReadOnlyList<string> context)
    {
        if (context.Count == 0)
        {
            _renderer.RenderHelp(_helpProvider.GetCommandHelp("help"));
            return ExitCodes.Success;
        }

        string command = context[0].ToLower();
        if (!_commandNames.Contains(command))
        {
            AnsiConsole.WriteLine("Unknown Command: {0}", command);
            return ExitCodes.GeneralError;
        }

        _renderer.RenderHelp(_helpProvider.GetCommandHelp(command));
        return ExitCodes.Success;

    }
}
