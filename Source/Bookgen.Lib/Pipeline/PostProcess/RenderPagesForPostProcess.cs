//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Domain.PostProcess;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Rendering.Images;
using Bookgen.Lib.Rendering.Markdown;
using Bookgen.Lib.Rendering.Markdown.RenderInterop;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.PostProcess;

internal sealed class RenderPagesForPostProcess : PipeLineStep<PostProcessState>
{
    private readonly IMemoryCache _memoryCache;

    public RenderPagesForPostProcess(PostProcessState state, IMemoryCache memoryCache) : base(state)
    {
        _memoryCache = memoryCache;
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        var imgConfig = new ImageConfig()
        {
            ResizeAndRecodeImages = ImgRecodeOption.Passtrough,
            SvgRecode = SvgRecodeOption.Passtrough,
        };

        var imgService = new ImgService(environment.Source, logger, imgConfig);

        var cached = new CachedImageService(imgService, _memoryCache);

        using var settings = new MarkdownRenderSettings(cached)
        {
            CssClasses = environment.Configuration.PrintConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            RenderInterop = new RenderInterop(environment, imgConfig),
            OffsetHeadingsBy = 1,
            AutoEmbedSupportedLinks = false,
        };

        using var markdown = new MarkdownConverter(settings);

        State.Export = new PostProcessExport
        {
            BookTitle = environment.Configuration.BookTitle,
            Chapters = new List<ExportChapter>()
        };

        foreach (TocChapter chapter in environment.TableOfContents.Chapters)
        {
            ExportChapter exportChapter = new ExportChapter
            {
                Title = chapter.Title,
                Items = new List<ChapterItem>()
            };

            foreach (var file in chapter.Files)
            {
                logger.LogDebug("Rendering {file}...", file);

                SourceFile sourceData = await environment.Source.GetSourceFile(file, logger);

                exportChapter.Items.Add(new ChapterItem
                {
                    Title = sourceData.FrontMatter.Title,
                    Tags = sourceData.FrontMatter.TagArray,
                    Html = markdown.RenderMarkdownToHtml(sourceData.Content),
                });
            }

            State.Export.Chapters.Add(exportChapter);
        }

        return StepResult.Success;
    }
}
