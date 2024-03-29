﻿//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Terminal;

namespace BookGen.DomainServices;

public static class GitParser
{
    private static readonly string[] Splits = new string[] { "\n" };

    public static GitStatus ParseStatus(string status)
    {
        string[] lines = status.Split(Splits, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 4)
            return new GitStatus();

        string[] inout = Extract(lines[3], "# branch.ab ").Split(' ');

        return new GitStatus
        {
            LastCommitId = Extract(lines[0], "# branch.oid "),
            BranchName = Extract(lines[1], "# branch.head "),
            IncommingCommits = int.Parse(inout[1]) * -1,
            OutGoingCommits = int.Parse(inout[0]),
            NotCommitedChanges = lines.Length - 4,
        };
    }

    private static string Extract(string line, string begining)
    {
        return line.StartsWith(begining)
            ? line[begining.Length..].Trim()
            : string.Empty;
    }
}
