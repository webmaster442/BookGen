//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("subcommands")]
internal class SubCommandsCommand : Command
{
    private readonly IEnumerable<IGrouping<char, string>> _commands;
    private readonly ITerminal _terminal;

    public SubCommandsCommand(IModuleApi api, ITerminal terminal)
    {
        _terminal = terminal;
        _commands = api
            .GetCommandNames()
            .Order()
            .GroupBy(x => x[0]);
    }

    public override int Execute(string[] context)
    {
        _terminal.Header("Available sub commands:");
        foreach (var commandGroup in _commands)
        {
            Console.WriteLine(commandGroup.Key);
            foreach (var command in commandGroup)
            {
                Console.WriteLine($"  {command}");
            }
        }
        return Constants.Succes;
    }
}
