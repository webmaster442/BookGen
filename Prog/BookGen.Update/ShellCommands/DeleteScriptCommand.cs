//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;

namespace BookGen.Update.ShellCommands;

internal class DeleteScriptCommand : IShellCommand
{
    public string ToBash() => "rm `basename \"$0\"`";

    public string ToPowerShell() => "Remove-Item -Force \"$PSCommandPath\"";
}