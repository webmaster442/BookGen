//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("version")]
[Description("Print the current program and config API version.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class VersionCommand : Command
{
    private readonly ProgramInfo _programInfo;

    public VersionCommand(ProgramInfo programInfo)
    {
        _programInfo = programInfo;
    }

    public override int Execute(IReadOnlyList<string> context)
    {
        AnsiConsole.WriteLine($"Version: {_programInfo.ProgramVersion}");
        AnsiConsole.WriteLine($"Config version: {_programInfo.ConfigVersion}");
        return ExitCodes.Success;
    }
}
