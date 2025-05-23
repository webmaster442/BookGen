//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Terminal;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("subcommands")]
internal class SubCommandsCommand : Command
{
    private readonly IEnumerable<IGrouping<char, string>> _commands;

    public SubCommandsCommand(ICommandRunnerProxy runnerProxy)
    {
        _commands = runnerProxy
            .CommandNames
            .Order()
            .GroupBy(cmd => cmd[0]);
    }

    public override int Execute(IReadOnlyList<string> context)
    {
        Terminal.Header("Available sub commands:");
        foreach (var commandGroup in _commands)
        {
            AnsiConsole.WriteLine(commandGroup.Key);
            foreach (var command in commandGroup)
            {
                AnsiConsole.WriteLine($"  {command}");
            }
        }
        return ExitCodes.Succes;
    }
}
