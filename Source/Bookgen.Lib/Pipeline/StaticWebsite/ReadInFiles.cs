using Bookgen.Lib.Domain;
using Bookgen.Lib.Internals;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class ReadInFiles : IPipeLineStep<StaticWebState>
{
    public ReadInFiles(StaticWebState state)
    {
        State = state;
    }

    public StaticWebState State { get; }

    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Reading in files...");
        var files = environment.TableOfContents.Chapters.SelectMany(x => x.Files);
        await Parallel.ForEachAsync(files, cancellationToken, async (file, token) =>
        {
            if (token.IsCancellationRequested) return;

            SourceFile sourceData = await environment.Source.GetSourceFile(file, logger);
            State.SourceFiles.TryAdd(file, sourceData);
        });

        return StepResult.Success;
    }
}
