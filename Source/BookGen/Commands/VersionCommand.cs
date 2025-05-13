//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices.Marshalling;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("version")]
internal sealed class VersionCommand : Command
{
    private readonly ProgramInfo _programInfo;

    public VersionCommand(ProgramInfo programInfo)
    {
        _programInfo = programInfo;
    }

    public override int Execute(string[] context)
    {
        AnsiConsole.WriteLine($"Version: {_programInfo.ProgramVersion}");
        AnsiConsole.WriteLine($"Config version: {_programInfo.ConfigVersion}");
        return ExitCodes.Succes;
    }
}
