//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookGen.Utilities
{
    internal static class MarkdownUtils
    {
        /// <summary>
        /// List files to process
        /// </summary>
        /// <param name="summaryFile">SUMMARY.md content</param>
        /// <returns>List of files</returns>
        public static TOC ParseToc(string summaryContent)
        {
            TOC toc = new TOC();

            using (var reader = new StringReader(summaryContent))
            {
                string line;
                //catch markdown links
                Regex myRegex = new Regex(@"(\* )\[(.+)\]\((.+)\)",
                                          RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

                string currentchapter = null;
                List<HtmlLink> chapter = null;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    //skip empty lines
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    if (line.StartsWith("#"))
                    {
                        InsertChapter(toc, ref currentchapter, ref chapter);
                        currentchapter = line.Replace("#", "");
                        chapter = new List<HtmlLink>(50);
                    }
                    else
                    {
                        var parts = from part in myRegex.Split(line)
                                    where
                                        !string.IsNullOrWhiteSpace(part) &&
                                        part.Trim() != "*"
                                    select
                                        part;

                        var final = parts.ToArray();
                        var link = new HtmlLink(final[0], final[1]);
                        chapter.Add(link);
                    }
                }
                InsertChapter(toc, ref currentchapter, ref chapter);
            }

            return toc;
        }

        private static void InsertChapter(TOC toc, ref string currentchapter, ref List<HtmlLink> chapter)
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
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("# ") ||
                        line.StartsWith("## ") ||
                        line.StartsWith("### "))
                        return line.Replace("#", "");
                }
            }
            return string.Empty;
        }
    }
}
