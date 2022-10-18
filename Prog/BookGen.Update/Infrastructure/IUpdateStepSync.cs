namespace BookGen.Update.Infrastructure;

internal interface IUpdateStepSync : IUpdateStep
{
    bool Execute(GlobalState state);
}
