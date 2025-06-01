using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.PostProcess;

internal sealed class WriteFile : PipeLineStep<PostProcessState>
{
    public WriteFile(PostProcessState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
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
