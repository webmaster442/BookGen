//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline;

public abstract class PipeLineStep<TState> : IPipeLineStep
{
    public PipeLineStep(TState state)
    {
        State = state;
    }

    public TState State { get; }

    public abstract Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger);
}
