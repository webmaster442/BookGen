using Bookgen.Lib.Pipeline.Print;
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

    public static Pipeline CratePrintPipeLine()
    {
        var state = new PrintState();

        return new PipeLineWithState<PrintState>(
            new RenderPages(state),
            new WriteHtml(state),
            new WriteXHtml(state)
        );
    }

    public static Pipeline CreateWebPipeLine()
    {
        var state = new StaticWebState();

        return new PipeLineWithState<StaticWebState>(
            new CopyAssets(state),
            new ExtractTemplateAssets(state),
            new ReadInFiles(state),
            new RenderStaticPages(state),
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
