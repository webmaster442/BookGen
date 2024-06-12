//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Cdg;

namespace BookGen.Shell.Commands;

[CommandName("cdg")]
internal sealed class CdgCommand : AsyncCommand<CdgArguments>
{
    public override async Task<int> Execute(CdgArguments arguments, string[] context)
    {
        var menu = new Cdg.CdgSelector(arguments.Folder);
        await menu.ShowMenu();
        return 0;
    }
}