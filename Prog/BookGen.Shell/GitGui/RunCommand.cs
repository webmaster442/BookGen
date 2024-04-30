//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;

namespace BookGen.Shell.GitGui;

internal class RunCommand : GuiCommand
{
    private readonly string[] _arguments;
    private readonly Func<bool>? _confirmCallback;

    public RunCommand(string name, string[] args, Func<bool>? confirmCallback = null)
    {
        DisplayName = name;
        _arguments = args;
        _confirmCallback = confirmCallback;
    }

    public override string DisplayName { get; }

    public override int Execute(string workDir, IProgress<string> progress)
    {
        if (_confirmCallback is not null)
        {
            if (_confirmCallback())
            {
                return ProcessRunner.RunProcess("git", _arguments, workDir, progress);
            }
            return 0;
        }
        else
        {
            return ProcessRunner.RunProcess("git", _arguments, workDir, progress);
        }
    }
}
