//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("help")]
[Description("Displays help information about the specified command.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.GeneralError, "The command failed.")]
//TODO: Add proper argument class
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
