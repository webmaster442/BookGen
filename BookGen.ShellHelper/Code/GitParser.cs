//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ShellHelper.Domain;
using System;

namespace BookGen.ShellHelper.Code
{
    internal static class GitParser
    {
        public static GitStatus ParseStatus(string status)
        {
            string[] lines = status.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length != 5)
                return new GitStatus();

            string[] inout = Extract(lines[3], "# branch.ab ").Split(' ');

            return new GitStatus
            {
                LastCommitId = Extract(lines[0], "# branch.oid "),
                BranchName = Extract(lines[1], "# branch.head "),
                IncommingCommits = int.Parse(inout[1]) * -1,
                OutGoingCommits = int.Parse(inout[0]),
            };
        }

        private static string Extract(string line, string begining)
        {
            if (line.StartsWith(begining))
            {
                return line[begining.Length..].Trim();
            }
            return string.Empty;
        }
    }
}
