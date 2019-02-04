//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using Markdig;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BookGen.Utilities
{
    internal static class MarkdownUtils
    {
        private static MarkdownPipeline _pipeline;

        static MarkdownUtils()
        {
            _pipeline = new MarkdownPipelineBuilder().Use<MarkdownModifier>().UseAdvancedExtensions().Build();
        }

        /// <summary>
        /// Generate markdown to html
        /// </summary>
        /// <param name="md">Markdown input string</param>
        /// <returns>html page</returns>
        public static string Markdown2HTML(string md)
        {
            return Markdown.ToHtml(md, _pipeline);
        }

        /// <summary>
        /// Generate markdown to plain text
        /// </summary>
        /// <param name="md">Markdown input string</param>
        /// <returns>plain text</returns>
        public static string Markdown2Plain(string md)
        {
            return Markdown.ToPlainText(md, _pipeline);
        }

        /// <summary>
        /// List files to process
        /// </summary>
        /// <param name="summaryFile">SUMMARY.md content</param>
        /// <returns>List of files</returns>
        public static List<string> GetFilesToProcess(string summaryContent)
        {
            var FilesToProcess = new List<string>(100);

            using (var reader = new StringReader(summaryContent))
            {
                string line;
                Regex myRegex = new Regex(@"\*\ \[.+\]\(",
                                          RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

                while ((line = reader.ReadLine()) != null)
                {
                    //skip empty lines
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                    line = line.Trim();

                    line = myRegex.Replace(line, string.Empty);
                    line = line.Substring(0, line.Length - 1);
                    FilesToProcess.Add(line);
                }
            }

            return FilesToProcess;
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
