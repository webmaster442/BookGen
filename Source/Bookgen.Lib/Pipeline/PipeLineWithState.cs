namespace Bookgen.Lib.Pipeline;

public sealed class PipeLineWithState<TState> : Pipeline
{
    protected override IEnumerable<IPipeLineStep> Steps { get; }

    public PipeLineWithState(params IPipeLineStep<TState>[] steps)
    {
        Steps = steps;
    }
}