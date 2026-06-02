//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Markdown.RenderInterop;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Print;

internal sealed class RenderPages : PipeLineStep<PrintState>
{
    private readonly IMemoryCache _memoryCache;

    public RenderPages(PrintState state, IMemoryCache memoryCache) : base(state)
    {
        _memoryCache = memoryCache;
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        var imgService = new ImgService(environment.Source, logger, environment.Configuration.PrintConfig.Images);
        var cached = new CachedImageService(imgService, _memoryCache);

        using var settings = new MarkdownRenderSettings(cached)
        {
            CssClasses = environment.Configuration.PrintConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            RenderInterop = new RenderInterop(environment, environment.Configuration.PrintConfig.Images),
            OffsetHeadingsBy = 1,
            AutoEmbedSupportedLinks = false,
        };

        using var markdown = new MarkdownConverter(settings);

        foreach (TocChapter chapter in environment.TableOfContents.Chapters)
        {
            logger.LogInformation("Rendering chapter {chapter}...", chapter.Title);
            State.Buffer.AppendH1(chapter.Title);
            State.Buffer.AppendLine("<section>");

            foreach (var page in chapter.Files)
            {
                logger.LogDebug("Rendering {file}...", page);

                SourceFile sourceData = await environment.Source.GetSourceFile(page, logger);

                State.Buffer.Append(markdown.RenderMarkdownToHtml(sourceData.Content));
                State.Buffer.AppendLine("</section>");
            }
        }

        return StepResult.Success;
    }
}
