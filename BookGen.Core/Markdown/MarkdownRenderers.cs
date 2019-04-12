//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Markdown.Pipeline;
using Markdig;

namespace BookGen.Core.Markdown
{
    public static class MarkdownRenderers
    {
        private static MarkdownPipeline _webpipeline;
        private static MarkdownPipeline _printpipeline;
        private static MarkdownPipeline _plainpipeline;

        static MarkdownRenderers()
        {
            _plainpipeline = new MarkdownPipelineBuilder().Use<WebModifier>().Build();
            _webpipeline = new MarkdownPipelineBuilder().Use<WebModifier>().UseAdvancedExtensions().Build();
            _printpipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Use<PrintModifier>().Build();
        }

        /// <summary>
        /// Generate markdown to html
        /// </summary>
        /// <param name="md">Markdown input string</param>
        /// <returns>html page</returns>
        public static string Markdown2WebHTML(string md)
        {
            return Markdig.Markdown.ToHtml(md, _webpipeline);
        }

        /// <summary>
        /// Generate markdown to plain text
        /// </summary>
        /// <param name="md">Markdown input string</param>
        /// <returns>plain text</returns>
        public static string Markdown2Plain(string md)
        {
            return Markdig.Markdown.ToPlainText(md, _plainpipeline);
        }

        public static string Markdown2PrintHTML(string md)
        {
            return Markdig.Markdown.ToHtml(md, _printpipeline);
        }
    }
}
