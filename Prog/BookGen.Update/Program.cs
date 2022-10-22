//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;
using BookGen.Update.Steps;

static void WriteIssues(List<string> issues)
{
    var current = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Yellow;
    foreach (var issue in issues)
    {
        Console.WriteLine(issue);
    }
    Console.ForegroundColor = current;
}

static void WriteException(Exception ex)
{
    var current = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ForegroundColor = current;
}

IUpdateStep[] steps = new IUpdateStep[]
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

foreach (var step in steps)
{
    try
    {
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

