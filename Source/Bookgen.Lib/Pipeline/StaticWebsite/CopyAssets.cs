using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

/// <summary>
/// Copy assets specified in configuration to output directory
/// </summary>
internal class CopyAssets : IPipeLineStep
{
    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        foreach (var asset in environment.Configuration.StaticWebsiteConfig.CopyToOutput)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning("Cancellation requested. Stoping...");
                return StepResult.Failure;
            }

            await environment.Source.CopyToAsync(asset, environment.Output.Scope)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        return StepResult.Success;
    }
}
