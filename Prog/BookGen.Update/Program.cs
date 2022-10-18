using BookGen.Update.Infrastructure;
using BookGen.Update.Steps;

IUpdateStep[] steps = new IUpdateStep[]
{
    new WelcomeStep(),

};

bool canContinue = true;
List<string> issues = new();

foreach (var step in steps)
{
    if (step is IUpdateStepAsync asyncStep)
        canContinue = await asyncStep.Execute(issues);
    else if (step is IUpdateStepSync syncStep)
        canContinue = syncStep.Execute(issues);

    if (!canContinue)
    {
        WriteIssues(issues);
        break;
    }
}

void WriteIssues(List<string> issues)
{
    throw new NotImplementedException();
}