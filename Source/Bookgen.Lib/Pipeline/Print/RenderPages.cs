//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Print;

internal sealed class RenderPages : PipeLineStep<PrintState>
{
    public RenderPages(PrintState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        var imgService = new ImgService(environment.Source, logger, environment.Configuration.PrintConfig.Images);
        var cached = new CachedImageService(imgService);

        using var settings = new RenderSettings(cached)
        {
            CssClasses = environment.Configuration.PrintConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            PrismJsInterop = new PrismJsInterop(environment),
            OffsetHeadingsBy = 1,
            AutoEmbedSupportedLinks = false
        };

        using var markdown = new MarkdownConverter(settings);

        foreach (var chapter in environment.TableOfContents.Chapters)
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
