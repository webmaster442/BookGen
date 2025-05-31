using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.PostProcess;

internal sealed class WriteFile : IPipeLineStep<PostProcessState>
{
    public WriteFile(PostProcessState state)
    {
        State = state;
    }

    public PostProcessState State { get; }

    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        if (State.Export is null)
        {
            logger.LogError("Export is null. Cannot write file.");
            return StepResult.Failure;
        }

        await environment.Output.SerializeAsync("export.json", State.Export, writeSchema: true);
        logger.LogInformation("Export written to {path}", "export.json");

        return StepResult.Success;
    }
}
