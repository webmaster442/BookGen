//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Spectre.Console;

namespace BookGen.Shellprog;

[CommandName("prompt")]
internal sealed class PromptCommand : GitCommandBase
{
    public PromptCommand(IAnsiConsole console) : base(console)
    {
    }

    public override int Execute(GitArguments arguments, IReadOnlyList<string> context)
    {
        if (!string.IsNullOrEmpty(arguments.WorkDirectory))
        {
            GitDirectoryStatus result = TestIfGitDir(arguments.WorkDirectory);
            if (result == GitDirectoryStatus.GitDirectory)
            {
                GitStatus? status = GetGitStatus(arguments.WorkDirectory);
                PrintStatus(status);
            }
            else if (result == GitDirectoryStatus.UntrustedGitDirectory)
            {
                PrintUntrusted();
            }
        }

        return 0;
    }
}
