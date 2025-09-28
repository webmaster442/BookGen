//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ServiceModel.Syndication;

using Bookgen.Lib.Domain;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Feed;

internal sealed class CreateItems : PipeLineStep<SyndicationFeedState>
{
    public CreateItems(SyndicationFeedState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        var imgService = new ImgService(environment.Source, logger, environment.Configuration.FeedConfig.Images);
        var cached = new CachedImageService(imgService);

        using var settings = new RenderSettings(cached)
        {
            CssClasses = environment.Configuration.FeedConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            PrismJsInterop = environment.Configuration.FeedConfig.PreRenderCode ? new PrismJsInterop(environment) : null,
            OffsetHeadingsBy = 0,
            AutoEmbedSupportedLinks = false
        };

        using var markdown = new MarkdownToHtml(settings);

        var renderer = new TemplateEngine(logger, environment);

        List<SyndicationItem> items = new();

        foreach (var chapter in environment.TableOfContents.Chapters)
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
