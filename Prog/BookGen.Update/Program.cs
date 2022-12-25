//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;
using BookGen.Update.Steps;
using System.Diagnostics;
using static BookGen.Update.ShellCommands.ShellFileGenerator;

static void WriteIssues(List<string> issues)
{
    ConsoleColor current = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Yellow;
    foreach (string issue in issues)
    {
        Console.WriteLine(issue);
    }
    Console.ForegroundColor = current;
}

static void WriteException(Exception ex)
{
    ConsoleColor current = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ForegroundColor = current;
}

var steps = new IUpdateStep[]
{
    new WelcomeStep(),
    new DownloadReleaseInfo(),
    new CheckIfUpdateNeeded(),
    new DownloadLatestRelease(),
    new VerifyHash(),
    new StopBookGenProcesses(),
    new ExtractZipPackage(),
    new Finish(),
};

bool canContinue = true;
GlobalState state = new();

state.Cleanup();

foreach (IUpdateStep step in steps)
{
    try
    {
        if (!string.IsNullOrEmpty(step.StatusMessage))
        {
            Console.WriteLine(step.StatusMessage);
        }

        if (step is IUpdateStepAsync asyncStep)
            canContinue = await asyncStep.Execute(state);
        else if (step is IUpdateStepSync syncStep)
            canContinue = syncStep.Execute(state);

        if (!canContinue)
        {
            WriteIssues(state.Issues);
            state.Cleanup();
            break;
        }
    }
    catch (Exception ex)
    {
        WriteException(ex);
    }
}

if (File.Exists(state.UpdateShellFileName))
{
    using (var p = new Process())
    {
        p.StartInfo.FileName = state.ShellType == ShellType.Bash
            ? "bash"
            : "powershell.exe";
        p.StartInfo.Arguments = state.ShellType == ShellType.Bash
            ? state.UpdateShellFileName
            : $"-ExecutionPolicy Bypass -File {state.UpdateShellFileName}";

        p.Start();
    }
}