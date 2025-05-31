using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Domain.PostProcess;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.PostProcess;

internal sealed class RenderPagesForPostProcess : IPipeLineStep<PostProcessState>
{
    public RenderPagesForPostProcess(PostProcessState state)
    {
        State = state;
    }

    public PostProcessState State { get; }

    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var imgService = new ImgService(environment.Source, new ImageConfig()
        {
            ResizeAndRecodeImagesToWebp = false,
            SvgRecode = SvgRecodeOption.Passtrough,
        });
        var cached = new CachedImageService(imgService);

        using var settings = new RenderSettings
        {
            CssClasses = environment.Configuration.PrintConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            PrismJsInterop = new PrismJsInterop(environment),
            OffsetHeadingsBy = 1,
        };

        using var markdown = new MarkdownToHtml(cached, settings);

        State.Export = new PostProcessExport
        {
            BookTitle = environment.Configuration.BookTitle,
            Chapters = new List<ExportChapter>()
        };

        foreach (var chapter in environment.TableOfContents.Chapters)
        {
            ExportChapter exportChapter = new ExportChapter
            {
                Title = chapter.Title,
                SubTitle = chapter.SubTitle,
                Items = new List<ChapterItem>()
            };

            foreach (var file in chapter.Files)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    logger.LogWarning("Cancellation requested. Stoping...");
                    return StepResult.Failure;
                }


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
