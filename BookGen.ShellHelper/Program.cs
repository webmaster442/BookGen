//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.ShellHelper.Code;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BookGen.Tests")]

namespace BookGen.ShellHelper
{
    public static class Program
    {
        private const string PromptMode = "prompt";
        private const int TimeOut = 1000;

        public static void Main(string[] arguments)
        {
            //Console.ReadKey();
            if (arguments.Length == 2
                && arguments[0] == PromptMode)
            {
                DoPrompt(arguments[1]);
            }

        }

        private static void DoPrompt(string workDir)
        {
            if (!string.IsNullOrEmpty(workDir)
                && TestIfGitDir(workDir))
            {
                var (exitcode, output) = ProcessRunner.RunProcess("git", "status -b -s --porcelain=2 --ignored MADRU", TimeOut);
                if (exitcode == 0)
                {
                    var status = GitParser.ParseStatus(output);
                    Console.WriteLine("({0}) ↓: {1} ↑:{2}", status.BranchName, status.IncommingCommits, status.OutGoingCommits);
                }
            }
        }

        private static bool TestIfGitDir(string workDir)
        {
            var (exitcode, _) = ProcessRunner.RunProcess("git", "status 2", TimeOut);
            return exitcode == 0;
        }
    }
}