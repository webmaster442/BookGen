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
internal sealed class HelpCommand : Command<HelpCommand.Arguments>
{
    internal sealed class Arguments : ArgumentsBase
    {
        [Argument(0, IsOptional = true)]
        [Description("The name of the command to display help for.")]
        public string CommandName { get; set; } = string.Empty;
    }

    private readonly HashSet<string> _commandNames;
    private readonly HelpRenderer _renderer = new();
    private readonly ICommandHelpProvider _commandHelpProvider;

    public HelpCommand(ICommandHelpProvider commandHelpProvider, ICommandRunnerProxy runnerProxy)
    {
        _commandNames = [.. runnerProxy.CommandNames];
        _commandHelpProvider = commandHelpProvider;
    }

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        if (string.IsNullOrEmpty(arguments.CommandName))
        {
            arguments.CommandName = "help";
        }
        if (!_commandNames.Contains(arguments.CommandName))
        {
            AnsiConsole.WriteLine("Unknown Command: {0}", arguments.CommandName);
            return ExitCodes.GeneralError;
        }

        string helpdocument = _commandHelpProvider.GetHelp(arguments.CommandName);

        if (Console.IsOutputRedirected)
        {
            AnsiConsole.WriteLine(helpdocument);
        }
        else
        {
            _renderer.RenderHelp(helpdocument.Split('\n'));
        }
        return ExitCodes.Success;

    }
}
