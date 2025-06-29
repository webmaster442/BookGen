using Bookgen.Lib.Domain.IO.Legacy;
using BookGen.Vfs;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Syntax.Inlines;

using Markdig.Syntax;

using Markdig;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Confighandling.Migration;

internal sealed class LoadLegacyToc : IMigrationStep
{
    private static ToC ParseToc(string content)
    {
        static void InsertChapter(ToC toc, ref string? currentchapter, ref List<Link>? chapter)
        {
            if (currentchapter != null && chapter != null)
            {
                toc.AddChapter(currentchapter, chapter);
                currentchapter = null;
                chapter = null;
            }
        }

        var parsed = new ToC();
        parsed.RawMarkdown = content;
        MarkdownPipeline? pipeline = new MarkdownPipelineBuilder().UseAutoIdentifiers(AutoIdentifierOptions.GitHub).Build();
        var doc = Markdig.Markdown.Parse(content, pipeline);

        string? chapterTitle = string.Empty;
        var chapterLinks = new List<Link>();
        foreach (MarkdownObject item in doc.Descendants())
        {
            if (item is HeadingBlock heading)
            {
                if (chapterLinks.Count > 0)
                {
                    InsertChapter(parsed, ref chapterTitle, ref chapterLinks);
                }
                chapterTitle = heading.Inline?.FirstChild?.ToString() ?? string.Empty;
                chapterLinks = new List<Link>(50);
            }
            else if (item is LinkInline link
                && !link.IsImage
                && link.FirstChild != null)
            {
                chapterLinks.Add(new Link(link.FirstChild.ToString()!, link.Url ?? string.Empty));
            }
        }
        InsertChapter(parsed, ref chapterTitle, ref chapterLinks);
        return parsed;
    }

    public async Task<bool> ExecuteAsync(IWritableFileSystem foler, MigrationState state, ILogger logger)
    {
        if (!foler.FileExists(state.LegacyConfig.TOCFile))
        {
            logger.LogWarning("Legacy TOC file not found");
            return false;
        }

        var toc = await foler.ReadAllTextAsync(state.LegacyConfig.TOCFile);
        state.LegacyToc = ParseToc(toc);

        return true;
    }
}