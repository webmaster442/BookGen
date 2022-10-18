namespace BookGen.Update.Infrastructure;

internal interface IUpdateStepAsync : IUpdateStep
{
    Task<bool> Execute(GlobalState state);
}
