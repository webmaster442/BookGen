//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Pipeline.Epub;
using Bookgen.Lib.Pipeline.Feed;
using Bookgen.Lib.Pipeline.PostProcess;
using Bookgen.Lib.Pipeline.Print;
using Bookgen.Lib.Pipeline.StaticWebsite;
using Bookgen.Lib.Pipeline.Wordpress;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline;

public sealed class Pipeline
{
    public Pipeline(params IPipeLineStep[] steps)
    {
        Steps = steps;
    }

    private IEnumerable<IPipeLineStep> Steps { get; }

    public async Task<bool> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        foreach (IPipeLineStep step in Steps)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning("Cancellation requested, stopping the pipeline execution.");
                return false;
            }

            StepResult result = await step.ExecuteAsync(environment, logger);
            if (result == StepResult.Failure)
            {
                return false;
            }
        }
        return true;
    }

    public static Pipeline CratePrintPipeLine(IMemoryCache memoryCache)
    {
        var state = new PrintState();

        return new Pipeline(
            new RenderHtmlPages(state, memoryCache),
            new WriteHtml(state),
            new WriteXHtml(state)
        );
    }

    public static Pipeline CreatePostProcessPipeLine(IMemoryCache memoryCache)
    {
        var state = new PostProcessState();

        return new Pipeline(
            new RenderPagesForPostProcess(state, memoryCache),
            new WriteFile(state)
        );
    }

    public static Pipeline CreateWebPipeLine(IMemoryCache memoryCache)
    {
        var state = new StaticWebState();

        return new Pipeline(
            new CopyAssets(state),
            new ExtractTemplateAssets(state),
            new ReadInFiles(state),
            new RenderTableOfContents(state),
            new RenderStaticPages(state, memoryCache),
            new RenderIndexPage(state, memoryCache),
            new RenderStabdaloneToc(state),
            new CreateEmptyIndexPagesForFolders(state),
            new GeneratePager(state)
        );
    }

    public static Pipeline CreateWordpressPipeLine(IMemoryCache memoryCache)
    {
        var state = new WpState();

        return new Pipeline(
            new CreateWpChannel(state),
            new CreateWpPages(state, memoryCache),
            new WriteExportFile(state)
        );
    }

    public static Pipeline CreateEpubPileLine(IMemoryCache memoryCache)
    {
        var state = new EpubState();

        return new Pipeline(
            new Initialize(state),
            new CreateMimeType(state),
            new CreateContainer(state),
            new CreateHtmlPages(state, memoryCache),
            new CreateImageFiles(state),
            new CreateFontFiles(state),
            new CreateEpubCoverAndStyle(state),
            new CreateNav(state),
            new CreateContentOpf(state),
            new DeInitialize(state)
        );
    }


    public static Pipeline CreateFeedPipeline(IMemoryCache memoryCache)
    {
        var state = new SyndicationFeedState();
        return new Pipeline(
            new CreateFeed(state),
            new CreateItems(state, memoryCache),
            new WriteFeeds(state)
        );
    }
}
