﻿//-----------------------------------------------------------------------------
// (c) 2021-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;

using BookGen.Domain.Terminal;

namespace BookGen.DomainServices;

public static class GitParser
{
    private static readonly string[] Splits = new string[] { "\n" };

    public static GitStatus ParseStatus(string status)
    {
        string[] lines = status.Split(Splits, StringSplitOptions.RemoveEmptyEntries);

        int notCommited = lines.Count(x => !x.StartsWith('#'));

        string[] inout = Parse(lines, "# branch.ab ", "0 0").Split(' ');

        return new GitStatus
        {
            LastCommitId = Parse(lines, "# branch.oid ", ""),
            BranchName = Parse(lines, "# branch.head ", "Unknown brach"),
            IncommingCommits = int.Parse(inout[1]) * -1,
            OutGoingCommits = int.Parse(inout[0]),
            NotCommitedChanges = notCommited
        };
    }

    private static T Parse<T>(IEnumerable<string> lines, string header, T defaultValue)
        where T : IParsable<T>
    {
        var line = lines.FirstOrDefault(x => x.StartsWith(header));

        if (line == null)
        {
            return defaultValue;
        }

        var cleaned = line[header.Length..].Trim();
        return T.Parse(cleaned, CultureInfo.InvariantCulture);
    }
}
