//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Epub.Ncx;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace BookGen.GeneratorSteps;

internal sealed class CreateToCForWebsite : IGeneratorContentFillStep
{
    public IContent? Content { get; set; }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        DependencyException.ThrowIfNull(Content);

        log.LogInformation("Generating Table of Contents...");
        var toc = new StringBuilder();

        foreach (string? chapter in settings.TocContents.Chapters)
        {
            if (!string.IsNullOrEmpty(chapter))
            {
                toc.Append("<details open=\"true\">");
                toc.AppendFormat("<summary>{0}</summary>", chapter);
            }
            toc.Append("<ul>");
            foreach (Link? link in settings.TocContents.GetLinksForChapter(chapter))
            {
                toc.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", link.ConvertToLinkOnHost(settings.Configuration.HostName), link.Text);
            }
            toc.Append("</ul>");
            if (!string.IsNullOrEmpty(chapter))
            {
                toc.Append("</details>");
            }
        }




        Content.TableOfContents = toc.ToString();
        Content.TableOfContentsHtml = GetRawToc(settings);
    }

    private static string GetRawToc(IReadonlyRuntimeSettings settings)
    {
        using var pipeline = new BookGenPipeline(BookGenPipeline.Web);
        pipeline.InjectRuntimeConfig(settings);
        pipeline.SetSvgPasstroughTo(settings.Configuration.TargetWeb.ImageOptions.SvgPassthru);

        var doc = Markdig.Markdown.Parse(settings.TocContents.RawMarkdown, pipeline.MarkdownPipeline);

        foreach (MarkdownObject item in doc.Descendants())
        {
            if (item is LinkInline link
                    && !link.IsImage
                    && link.FirstChild != null)
            {
                link.Url = $"{settings.Configuration.HostName}{Path.ChangeExtension(link.Url, ".html")}";
            }
        }

        return Markdig.Markdown.ToHtml(doc, pipeline.MarkdownPipeline);
    }
}
