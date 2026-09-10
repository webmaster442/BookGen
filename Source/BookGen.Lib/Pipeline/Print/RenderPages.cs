//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Domain;
using BookGen.Lib.Domain.IO;
using BookGen.Lib.Rendering;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown;
using BookGen.Lib.Rendering.Markdown.RenderInterop;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen.Lib.Pipeline.Print;

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

        using var renderInterop = new RenderInterop(environment, environment.ProgramPathResolver, environment.Configuration.PrintConfig.Images);

        var settings = new MarkdownRenderSettings(cached)
        {
            CssClasses = environment.Configuration.PrintConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            RenderInterop = renderInterop,
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
