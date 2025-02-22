//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Interfaces;

using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Microsoft.Extensions.Logging;

namespace BookGen.DomainServices
{
    public static class MarkdownUtils
    {
        /// <summary>
        /// List files to process
        /// </summary>
        /// <param name="content">SUMMARY.md content</param>
        /// <returns>List of files</returns>
        public static ToC ParseToc(string content)
        {
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

        private static void InsertChapter(ToC toc, ref string? currentchapter, ref List<Link>? chapter)
        {
            if (currentchapter != null && chapter != null)
            {
                toc.AddChapter(currentchapter, chapter);
                currentchapter = null;
                chapter = null;
            }
        }

        /// <summary>
        /// Get title from markdown content
        /// </summary>
        /// <param name="markDownContent">markdown content</param>
        /// <returns>Title of page</returns>
        public static string GetDocumentTitle(string markDownContent, ILogger log, FsPath fileName)
        {
            using (var reader = new StringReader(markDownContent))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("# "))
                    {
                        return line[2..];
                    }
                    else if (line.StartsWith("## "))
                    {
                        log.LogWarning("Found 2nd level title as the document title: {line}, File: {fileName}", line, fileName);
                        return line[3..];
                    }
                    else if (line.StartsWith("### "))
                    {
                        log.LogWarning("Found 3rd level title as the document title: {line}, File: {fileName}", line, fileName);
                        return line[4..];
                    }
                    else if (line.StartsWith("#### "))
                    {
                        log.LogWarning("Found 4th level title as the document title: {line}, File: {fileName}", line, fileName);
                        return line[5..];
                    }
                    else if (line.StartsWith("##### "))
                    {
                        log.LogWarning("Found 5th level title as the document title: {line}, File: {fileName}", line, fileName);
                        return line[6..];
                    }
                    else if (line.StartsWith("###### "))
                    {
                        log.LogWarning("Found 6th level title as the document title: {line}, File: {fileName}", line, fileName);
                        return line[7..];
                    }
                }
            }

            log.LogWarning("Found no document title : {fileName}", fileName);
            return string.Empty;
        }
    }
}
