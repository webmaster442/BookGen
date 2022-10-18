namespace BookGen.Update.Infrastructure;

internal interface IUpdateStepSync : IUpdateStep
{
    bool Execute(IList<string> issues);
}
