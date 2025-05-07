using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal class CopyAssets : IPipeLineStep
{
    public async Task<StepResult> ExecuteAsync(IEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        foreach (var asset in environment.Configuration.StaticWebsiteConfig.AssetsToCopy)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning("Cancellation requested. Stoping...");
                return StepResult.Failure;
            }

            await environment.Source.CopyTo(asset, environment.Output).ConfigureAwait(continueOnCapturedContext: false);
        }

        return StepResult.Success;
    }
}
