//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels.Commands;

internal sealed class RunProgramCommand : ProcessCommandBase
{
    private readonly string _program;
    private readonly string _arguments;

    public RunProgramCommand(string program, string arguments)
    {
        _program = program;
        _arguments = arguments;
    }

    public override void Execute(string? folder)
    {
        if (string.IsNullOrEmpty(folder)
            || !Directory.Exists(folder))
        {
            Message(Properties.Resources.FolderNoLongerExists, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrEmpty(_arguments))
            RunProgram(_program, folder);
        else
            RunProgram(_program, _arguments + " " + folder);
    }
}
