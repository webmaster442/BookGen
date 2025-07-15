using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class DeInitialize : PipeLineStep<EpubState>
{
    public DeInitialize(EpubState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        State.Deinitialize();
        return Task.FromResult(StepResult.Success);
    }
}