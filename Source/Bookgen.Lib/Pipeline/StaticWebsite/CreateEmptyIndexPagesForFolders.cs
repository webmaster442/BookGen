using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class CreateEmptyIndexPagesForFolders : IPipeLineStep
{
    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var folders = environment.Output.GetDirectories(environment.Output.Scope, true);

        var protect = environment.GetAsset(BundledAssets.ProtectHtml);

        logger.LogInformation("Creating index.html files in subfolders...");

        foreach (var folder in folders)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning("Cancellation requested. Stoping...");
                return StepResult.Failure;
            }

            var filename = "index.html";
            logger.LogDebug("creating index.html in {folder}...", folder);
            await environment.Output.WriteAllTextAsync(filename, protect);
        }

        return StepResult.Success;
    }
}
