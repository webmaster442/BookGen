using Bookgen.Lib.Domain;
using Bookgen.Lib.Internals;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class ReadInFiles : PipeLineStep<StaticWebState>
{
    public ReadInFiles(StaticWebState staticWebState) : base(staticWebState)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Reading in files...");
        await Parallel.ForEachAsync(environment.TableOfContents.GetFiles(), cancellationToken, async (file, token) =>
        {
            if (token.IsCancellationRequested) return;

            SourceFile sourceData = await environment.Source.GetSourceFile(file, logger);
            State.SourceFiles.TryAdd(file, sourceData);
        });

        return StepResult.Success;
    }
}
