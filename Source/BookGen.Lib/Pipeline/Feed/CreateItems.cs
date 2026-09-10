//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ServiceModel.Syndication;

using BookGen.Lib.Domain;
using BookGen.Lib.Domain.IO;
using BookGen.Lib.Rendering;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown;
using BookGen.Lib.Rendering.Markdown.RenderInterop;
using BookGen.Lib.Rendering.Templates;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen.Lib.Pipeline.Feed;

internal sealed class CreateItems : PipeLineStep<SyndicationFeedState>
{
    private readonly IMemoryCache _memoryCache;

    public CreateItems(SyndicationFeedState state, IMemoryCache memoryCache) : base(state)
    {
        _memoryCache = memoryCache;
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        var imgService = new ImgService(environment.Source, logger, environment.Configuration.FeedConfig.Images);
        var cached = new CachedImageService(imgService, _memoryCache);

        using var renderInterop = new RenderInterop(environment, environment.ProgramPathResolver, environment.Configuration.FeedConfig.Images);

        var settings = new MarkdownRenderSettings(cached)
        {
            CssClasses = environment.Configuration.FeedConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            RenderInterop = renderInterop,
            OffsetHeadingsBy = 0,
            AutoEmbedSupportedLinks = false,
        };

        settings.RenderInterop.PreRenderCode = environment.Configuration.FeedConfig.PreRenderCode;

        using var markdown = new MarkdownConverter(settings);

        var renderer = new TemplateEngine(logger, environment);

        List<SyndicationItem> items = new();

        foreach (TocChapter chapter in environment.TableOfContents.Chapters)
        {
            logger.LogInformation("Rendering chapter {chapter}...", chapter.Title);

            foreach (var page in chapter.Files)
            {
                logger.LogDebug("Rendering {file}...", page);

                SourceFile sourceData = await environment.Source.GetSourceFile(page, logger);

                string template = await environment.GetTemplate(frontMatterTemplate: sourceData.FrontMatter.Template,
                                                                fallbackTemplate: BundledAssets.TemplateBlank,
                                                                defaultTemplateSelector: cfg => cfg.FeedConfig.DefaultTempate);

                var viewData = new ViewData
                {
                    Content = markdown.RenderMarkdownToHtml(sourceData.Content),
                    Title = sourceData.FrontMatter.Title,
                    Host = string.Empty,
                    LastModified = sourceData.LastModified,
                };

                string content = renderer.Render(template, viewData);

                items.Add(new SyndicationItem
                {
                    Title = new TextSyndicationContent(sourceData.FrontMatter.Title),
                    Content = new TextSyndicationContent(content, TextSyndicationContentKind.Html),
                    PublishDate = sourceData.LastModified,
                    LastUpdatedTime = sourceData.LastModified,
                });
            }

            State.Feed.Items = items;
        }

        return StepResult.Success;
    }
}
