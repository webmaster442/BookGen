//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using BookGen.Cli.Annotations;

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
            var result = TestIfGitDir(arguments.WorkDirectory);
            if (result == GitDirectoryStatus.GitDirectory)
            {
                var status = GetGitStatus(arguments.WorkDirectory);
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
