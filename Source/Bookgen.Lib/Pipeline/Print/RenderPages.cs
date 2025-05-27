using Bookgen.Lib.Domain;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Print;

internal sealed class RenderPages : IPipeLineStep<PrintState>
{
    public RenderPages(PrintState state)
    {
        State = state;
    }

    public PrintState State { get; }

    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var imgService = new ImgService(environment.Source, environment.Configuration.StaticWebsiteConfig.Images);
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

        foreach (var chapter in environment.TableOfContents.Chapters)
        {
            State.Buffer.AppendH1(chapter.Title);
            State.Buffer.AppendLine("<section>");

            foreach (var page in chapter.Files)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    logger.LogWarning("Cancellation requested. Stoping...");
                    return StepResult.Failure;
                }

                logger.LogDebug("Rendering {file}...", page);

                SourceFile sourceData = await environment.Source.GetSourceFile(page, logger);

                State.Buffer.Append(markdown.RenderMarkdownToHtml(sourceData.Content));
                State.Buffer.AppendLine("</section>");
            }
        }

        return StepResult.Success;
    }
}
