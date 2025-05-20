using Bookgen.Lib.Pipeline.StaticWebsite;
using Bookgen.Lib.Pipeline.Wordpress;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline;

public abstract class Pipeline
{
    protected abstract IEnumerable<IPipeLineStep> Steps { get; }

    public async Task<bool> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        foreach (var step in Steps)
        {
            var result = await step.ExecuteAsync(environment, logger, cancellationToken);
            if (result == StepResult.Failure)
            {
                return false;
            }
        }
        return true;
    }

    public static Pipeline CreateWebPipeLine()
    {
        var state = new StaticWebState();

        return new SimplePipeLine(
            new CopyAssets(state),
            new ExtractTemplateAssets(state),
            new RenderPages(state),
            new CreateEmptyIndexPagesForFolders(state)
        );
    }

    public static Pipeline CreateWordpressPipeLine()
    {
        var state = new WpState();

        return new PipeLineWithState<WpState>(
            new CreateWpChannel(state),
            new CreateWpPages(state),
            new WriteExportFile(state)
        );
    }
}
