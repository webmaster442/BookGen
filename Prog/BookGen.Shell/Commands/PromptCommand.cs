//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Annotations;
using BookGen.DomainServices;

using Spectre.Console;

namespace BookGen.Shell.Commands;

[CommandName("prompt")]
internal sealed class PromptCommand : GitCommandBase
{
    public PromptCommand(IAnsiConsole console) : base(console)
    {
    }

    public override int Execute(GitArguments arguments, string[] context)
    {
        if (!string.IsNullOrEmpty(arguments.WorkDirectory)
            && TestIfGitDir(arguments.WorkDirectory))
        {
            GetGitStatus(arguments.WorkDirectory);
        }
        return 0;
    }
}
