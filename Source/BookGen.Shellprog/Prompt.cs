//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

namespace BookGen.Shellprog;

[CommandName("prompt")]
internal sealed class Prompt : GitCommandBase<Prompt.Arguments>
{
    internal sealed class Arguments : GitArguments
    {
        [Argument(1, IsOptional = false)]
        [Description("Last command success status")]
        public bool WasSuccess { get; set; } = false;
    }

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        if (string.IsNullOrWhiteSpace(arguments.WorkDirectory))
            return 0;

        List<string> parts =
        [
            "╭╴",
            MakeLink(arguments.WorkDirectory, 110),
            "\r\n",
            "╰╴ PS ",
        ];

        GitDirectoryStatus result = TestIfGitDir(arguments.WorkDirectory);
        if (result == GitDirectoryStatus.UntrustedGitDirectory)
        {
            parts.Add(new TerminalCell
            {
                Background = TerminalColor.Black,
                Foreground = TerminalColor.Red,
                Text = "<untrusted>",
            });
        }
        else if (result == GitDirectoryStatus.GitDirectory)
        {
            GitStatus? status = GetGitStatus(arguments.WorkDirectory);
            if (status?.IncommingCommits > 0)
            {
                parts.Add(new TerminalCell
                {
                    Background = TerminalColor.Black,
                    Foreground = TerminalColor.Magenta,
                    Text = $"↓: {status.IncommingCommits}",
                });
            }
            if (status?.OutGoingCommits > 0)
            {
                parts.Add(new TerminalCell
                {
                    Background = TerminalColor.Black,
                    Foreground = TerminalColor.Yellow,
                    Text = $" ↑: {status.OutGoingCommits}",
                });
            }
            if (status?.NotCommitedChanges > 0)
            {
                parts.Add(new TerminalCell
                {
                    Background = TerminalColor.Black,
                    Foreground = TerminalColor.Red,
                    Text = $" M: {status.NotCommitedChanges}",
                });
            }
        }

        parts.Add(MakeLastExitStatusDisplay(arguments.WasSuccess));
        parts.Add(" >");

#pragma warning disable Spectre1000 // Use AnsiConsole instead of System.Console
        foreach (var part in parts)
        {
            Console.Write(part);
        }
#pragma warning restore Spectre1000 // Use AnsiConsole instead of System.Console

        return 0;

    }

    private static string MakeLastExitStatusDisplay(bool wasSuccess)
    {
        return wasSuccess
            ? "✅"
            : "❌";
    }

    private static string MakeLink(string workDirectory, int maxWidth)
    {
        var uri = new Uri(workDirectory).AbsoluteUri;
        string text = workDirectory.Length > maxWidth ? $"...{workDirectory[(workDirectory.Length - maxWidth + 3)..]}" : workDirectory;
        return $"\e]8;;{uri}\a{text}\e]8;;\a";
    }
}
