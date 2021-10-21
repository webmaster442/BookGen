//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.ShellHelper.Code;
using System;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BookGen.Tests")]

public static class Program
{
    private const string PromptMode = "prompt";

    public static void Main(string[] arguments)
    {
        if (arguments.Length == 2
            && arguments[0] == PromptMode)
        {
            DoPrompt(arguments[1]);
        }

    }

    private static void DoPrompt(string workDir)
    {
        if (string.IsNullOrEmpty(workDir)
            && Directory.Exists(Path.Combine(workDir, ".git")))
        {
            var (exitcode, output) = ProcessRunner.RunProcess("git", "status -b -s --porcelain=2 --ignored MADRU", 1000);
            if (exitcode == 0)
            {
                var status = GitParser.ParseStatus(output);
                Console.WriteLine("({0}) ↙: {1} ↗:{2}", status.BranchName, status.IncommingCommits, status.OutGoingCommits);
            }
        }
    }
}
