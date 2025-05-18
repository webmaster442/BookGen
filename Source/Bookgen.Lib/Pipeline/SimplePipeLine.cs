namespace Bookgen.Lib.Pipeline;

public sealed class SimplePipeLine : Pipeline
{
    protected override IEnumerable<IPipeLineStep> Steps { get; }

    public SimplePipeLine(params IPipeLineStep[] steps)
    {
        Steps = steps;
    }
}
