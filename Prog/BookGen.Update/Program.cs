using BookGen.Update.Infrastructure;
using BookGen.Update.Steps;

IUpdateStep[] steps = new IUpdateStep[]
{
    new WelcomeStep(),
    new DownloadReleaseInfo(),
    new CheckIfUpdateNeeded(),
};

bool canContinue = true;
GlobalState state = new();

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
            break;
        }
    }
    catch (Exception ex)
    {
        WriteException(ex);
    }
}

void WriteIssues(List<string> issues)
{
    var current = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Yellow;
    foreach (var issue in issues)
    {
        Console.WriteLine(issue);
    }
    Console.ForegroundColor = current;
}

void WriteException(Exception ex)
{
    var current = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ForegroundColor = current;
}