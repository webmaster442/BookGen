using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

/// <summary>
/// Extract embedded assets if, no default template is given
/// </summary>
internal sealed class ExtractTemplateAssets : IPipeLineStep<StaticWebState>
{
    public ExtractTemplateAssets(StaticWebState staticWebState)
    {
        State = staticWebState;
    }

    public StaticWebState State { get; }

    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(environment.Configuration.StaticWebsiteConfig.DefaultTempate))
        {
            logger.LogInformation("Custom template specified, skiping assset extraction...");
            return StepResult.Success;
        }

        logger.LogInformation("Extracting template assets...");

        logger.LogDebug("Extracting bookgen.min.css...");
        await environment.Output.WriteAllTextAsync(BundledAssets.BookGenCss, environment.GetAsset(BundledAssets.BookGenCss));

        logger.LogDebug("Extracting prism.js...");
        await environment.Output.WriteAllTextAsync(BundledAssets.PrismJs, environment.GetAsset(BundledAssets.PrismJs));

        return StepResult.Success;
    }
}
