//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;

namespace BookGen.Shell.GitGui;

internal class RunCommand : GuiCommand
{
    private readonly string[] _arguments;

    public RunCommand(string name, string[] args)
    {
        DisplayName = name;
        _arguments = args;
    }

    public override string DisplayName { get; }

    public override int Execute(string workDir, IProgress<string> progress) 
        => ProcessRunner.RunProcess("git", _arguments, workDir, progress);
}
