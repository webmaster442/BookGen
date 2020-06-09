//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BookGen.Utilities
{
    internal static class MarkdownUtils
    {
        private static readonly Regex _indexExpression = new Regex(@"(\[\^\d+\])", RegexOptions.Compiled);

        /// <summary>
        /// List files to process
        /// </summary>
        /// <param name="summaryFile">SUMMARY.md content</param>
        /// <returns>List of files</returns>
        public static ToC ParseToc(string content)
        {
            ToC parsed = new ToC();
            var pipeline = new MarkdownPipelineBuilder().UseAutoIdentifiers(AutoIdentifierOptions.GitHub).Build();
            var doc = Markdown.Parse(content, pipeline);

            string? chapterTitle = string.Empty;
            List<Link>? chapterLinks = new List<Link>();
            foreach (MarkdownObject item in doc.Descendants())
            {
                if (item is HeadingBlock heading)
                {
                    if (chapterLinks.Count > 0)
                    {
                        InsertChapter(parsed, ref chapterTitle, ref chapterLinks);
                    }
                    chapterTitle = heading.Inline.FirstChild.ToString();
                    chapterLinks = new List<Link>(50);
                }
                else if (item is LinkInline link && !link.IsImage)
                {
                    if (link.FirstChild != null)
                        chapterLinks.Add(new Link(link.FirstChild.ToString()!, link.Url));
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
        /// <param name="md">markdown content</param>
        /// <returns>Title of page</returns>
        public static string GetTitle(string md)
        {
            using (var reader = new StringReader(md))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("# "))
                        return line.Substring(2);
                    else if (line.StartsWith("## "))
                        return line.Substring(3);
                    else if (line.StartsWith("### "))
                        return line.Substring(4);
                }
            }
            return string.Empty;
        }

        public static string Reindex(string inputContent, ref int index)
        {
            int numMatches = _indexExpression.Matches(inputContent).Count;

            if (numMatches < 1)
                return inputContent;

            inputContent = _indexExpression.Replace(inputContent, "REG$0");
            for (int i = 0; i < (numMatches / 2); i++)
            {
                string expression = $"(REG\\[\\^{i + 1}\\])";
                Regex r = new Regex(expression, RegexOptions.Compiled);
                if (r.IsMatch(inputContent))
                {
                    inputContent = r.Replace(inputContent, $"[^{index}]");
                    ++index;
                }
            }

            return inputContent;
        }
    }
}
