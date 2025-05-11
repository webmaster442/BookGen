//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Spectre.Console;

namespace BookGen.Shellprog;

[CommandName("cmdlist")]
internal class CommandListCommand : Command
{
    private readonly CommandNameProvider _nameProvider;

    public CommandListCommand(CommandNameProvider nameProvider)
    {
        _nameProvider = nameProvider;
    }

    public override int Execute(string[] context)
    {
        AnsiConsole.WriteLine("BookGen Shell");
        AnsiConsole.WriteLine("Avaliable commands:");
        AnsiConsole.WriteLine();
        foreach (var item in _nameProvider.CommandNames) 
        {
            AnsiConsole.WriteLine(item);
        }
        return 0;
    }
}
